using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHandler : MonoBehaviour
{
    //==================================================

    [Header("메인 룰렛 멈추는 핸들러")]
    public StopSliderHandler stopHandler;

    [Header("캐스트 하는 스킬 띄워주는 핸들러")]
    public PieceCastUIHandler castUIHandler;

    private BattleInfoHandler battleInfoHandler;
    private CCHandler ccHandler;
    private BattleRewardHandler battleRewardHandler;
    private BattleTargetSelectHandler battleTargetSelector;

    [HideInInspector]
    public BattleScrollHandler battleScroll;
    [HideInInspector]
    public BattleUtilHandler battleUtil;
    [HideInInspector]
    public BattleEventHandler battleEvent;
    [HideInInspector]
    public BattleFieldHandler fieldHandler;

    private InventoryHandler inventory;

    //==================================================

    private BattleInfo battleInfo;

    [Header("적을 생성하는 위치")]
    public Transform createTrans;

    // 첫턴 룰렛이 꽉 채워지고 돌아갈 때
    [HideInInspector]
    public bool isBattle = false;

    // 진짜 스타트 함수를 불렀을 때 바로
    [HideInInspector]
    public bool isBattleStart = false;

    public bool canPause = false;

    //==================================================

    [Header("룰렛들")]

    // 메인 룰렛
    public SkillRullet mainRullet;

    // 결과로 나온 룰렛조각
    [HideInInspector]
    public SkillPiece result;
    private int resultIdx;

    //==================================================

    [Header("플레이어&적 Health")]
    public PlayerHealth player;
    public Transform playerImgTrans;
    public Transform playerHpbarTrans;

    //[HideInInspector]
    public List<EnemyHealth> enemys = new();

    //==================================================

    private int turnCnt = 0;

    //==================================================

    public Transform bottomPos;

    //==================================================

    [Header("현재 맵 보스 타입")]
    [HideInInspector] public BattleInfo _bossInfo = null;
    public bool isElite = false;
    public bool isBoss = false;

    [Header("UI들")]
    public BottomUIHandler bottomUI;
    public Image bg;

    public BattleFadeUIHandler battleFade;

    public CinemachineVirtualCamera cvCam;

    #region WaitSeconds
    private readonly WaitForSeconds oneSecWait = new(1f);
    private readonly WaitForSeconds pFiveSecWait = new(0.5f);
    private readonly WaitForSeconds pOneSecWait = new(0.1f);
    private List<bool> boolList = new() { true, false };
    #endregion

    private void Awake()
    {
        GameManager.Instance.battleHandler = this;

        battleInfoHandler = GetComponent<BattleInfoHandler>();
        ccHandler = GetComponent<CCHandler>();
        battleRewardHandler = GetComponent<BattleRewardHandler>();
        battleTargetSelector = GetComponent<BattleTargetSelectHandler>();
        battleUtil = GetComponent<BattleUtilHandler>();
        battleScroll = GetComponent<BattleScrollHandler>();
        battleEvent = GetComponent<BattleEventHandler>();
        fieldHandler = GetComponent<BattleFieldHandler>();

        GameManager.Instance.SetResolution();
    }

    private void Start()
    {
        inventory = GameManager.Instance.inventoryHandler;

        // 플레이어가 가지고 있는 기본 스킬 생성 일단 테스트로 만들어놈
        player.GetComponent<Inventory>().CreateSkills();
    }

    #region StartBattle

    // 전투를 시작하는 함수
    public void StartBattle(bool isElite = false, bool isBoss = false, BattleInfo bInfo = null)
    {
        this.isElite = isElite;
        this.isBoss = isBoss;

        if (isBattleStart)
        {
            //Debug.LogError("이미 전투가 진행중입니다!");
            return;
        }
        isBattleStart = true;
        GameManager.Instance.encounterHandler.ShowBlackPanel(false);

        bottomUI.ShowBottomPanel(true);

        GameManager.Instance.goldUIHandler.ShowGoldUI();
        GetComponent<BattleScrollHandler>().ShowScrollUI();
        GameManager.Instance.bottomUIHandler.ShowBottomPanel(true);
        //print("전투시작");
        SoundHandler.Instance.PlayBGMSound("Battle_4");

        if (bInfo != null)
        {
            battleInfo = bInfo;
        }
        else if (isBoss)
        {
            battleInfo = _bossInfo.enemyInfos.Count > 0 ? _bossInfo : battleInfoHandler.GetRandomBossInfo();
        }
        else if (isElite)
        {
            battleInfo = battleInfoHandler.GetRandomEliteInfo();
        }
        else
        {
            //print("적 가져옴");
            battleInfo = battleInfoHandler.GetRandomBattleInfo();
        }

        bg.sprite = battleInfo.bg;


        battleFade.ShowEffect();

        // 적 생성
        CreateEnemy(battleInfo.enemyInfos, () =>
        {
            // 전투가 시작하기 전 인벤토리와 룰렛 정리
            StartCoroutine(InitRullet());
            StartCoroutine(battleEvent.ActionEvent(EventTime.BeginBattle));
        });

        // 핸들러들 초기화
        InitHandler();

        // 스탑 버튼에 기능 추가
        SetStopHandler();

        LogCon log = new()
        {
            text = "전투 시작",
            hasLine = true
        };

        DebugLogHandler.AddLog(LogType.OnlyText, log);
    }

    private void InitHandler()
    {
        battleRewardHandler.Init(GameManager.Instance.skillContainer.playerSkillPrefabs);
        battleUtil.Init(mainRullet);
    }

    public void CreateEnemy(List<EnemyType> enemyInfos, Action onCreateEnd) //다중생성
    {
        List<EnemyHealth> createdEnemy = new();

        for (int i = 0; i < enemyInfos.Count; i++)
        {
            EnemyHealth enemy = PoolManager.GetEnemy(enemyInfos[i]);
            enemy.Init();
            enemy.transform.position = createTrans.position;

            enemys.Add(enemy);
            createdEnemy.Add(enemy);
        }

        // 보스면 가운데와 바꿔줘야된다 
        SortBoss();
        SetEnemyPosition();
        SetSize();

        // 스킬 생성
        StartCoroutine(CreateEnemySkill(createdEnemy, onCreateEnd));
    }

    private void SortBoss()
    {
        int idx = enemys.Count / 2;

        for (int i = 0; i < enemys.Count; i++)
        {
            // 보스면
            if (enemys[i].isBoss && i != idx)
            {
                (enemys[i], enemys[idx]) = (enemys[idx], enemys[i]);
            }
        }
    }

    private void SetSize()
    {
        if (enemys.Count >= 3)
        {
            for (int i = 0; i < enemys.Count; i++)
            {
                if (i == 0 || i == enemys.Count - 1)
                {
                    enemys[i].SetScale(0.75f);
                }
                else
                {
                    enemys[i].SetScale(1f);
                }
            }
        }
        else
        {
            for (int i = 0; i < enemys.Count; i++)
            {
                enemys[i].SetScale(1f);
            }
        }
    }

    public void SetEnemyPosition()
    {
        if (enemys.Count <= 0)
        {
            return;
        }

        int enemyCount = 0;

        for (int i = 0; i < enemys.Count; i++)
        {
            if (!enemys[i].IsDie)
            {
                enemyCount++;
            }
        }

        Vector2 screenX = new(Camera.main.ViewportToWorldPoint(Vector3.zero).x, Camera.main.ViewportToWorldPoint(Vector3.one).x);
        float posX = (Mathf.Abs(screenX.x) + screenX.y) / (enemyCount + 1);

        // -10  -5   0   5  10
        // 

        float totalX = 0f;

        for (int i = 0; i < enemys.Count; i++)
        {
            if (!enemys[i].IsDie)
            {
                totalX += posX;

                enemys[i].transform.DOMoveX(totalX + screenX.x, 0.3f);
            }
        }
    }

    private void HideEnemyIndicator()
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            enemys[i].indicator.HideText();
        }
    }

    private void SetStopHandler()
    {
        if (stopHandler == null)
        {
            Debug.LogError("인스펙터에서 BatteHandler에 스탑 핸들러를 추가해주세요!!");
            return;
        }

        stopHandler.Init(mainRullet, (result, pieceIdx) =>
        {
            SetInteract(false);

            stopHandler.rullet.HighlightResult();

            this.result = result as SkillPiece;
            resultIdx = pieceIdx;

            // 결과를 저장해놓고 그 칸을 빈칸으로 만들어준다
            battleUtil.SetRulletEmpty(resultIdx);

            // 결과 실행
            CastPiece(this.result);
        },
        () =>
        {
            // 스크롤 버튼 비활성화
            SetInteract(false);
        });
    }

    #endregion

    #region Init Rullet

    private IEnumerator InitRullet()
    {
        yield return null;

        // 인벤토리에서 랜덤한 6개의 스킬을 뽑아 룰렛에 적용한다. 단, 최소한 적의 스킬 1개와 내 스킬 2개가 보장된다.

        yield return StartCoroutine(battleUtil.ResetRulletPiecesWithCondition(hasWait: true));

        yield return pFiveSecWait;

        isBattle = true;

        // 턴 시작
        InitTurn();
    }

    private IEnumerator CreateEnemySkill(List<EnemyHealth> enemys, Action onCreateEnd = null)
    {
        // 적의 스킬을 추가해준다
        float maxTime = 0;

        for (int i = 0; i < enemys.Count; i++)
        {
            float time = enemys[i].GetComponent<EnemyInventory>().CreateSkillsSmooth();

            if (maxTime < time)
            {
                maxTime = time;
            }
        }

        yield return new WaitForSeconds(maxTime + 0.25f);

        onCreateEnd?.Invoke();

        yield break;
    }
    #endregion

    #region Turns

    // 다음 턴으로 넘어가는 것
    private void InitTurn()
    {
        // 위치 초기화
        SetEnemyPosition();
        SetSize();

        HideEnemyIndicator();

        // 버튼 초기화
        stopHandler.SetInteract(false);

        result = null;

        turnCnt++;

        LogCon log = new()
        {
            text = "",
            hasLine = true
        };

        DebugLogHandler.AddLog(LogType.OnlyText, log);

        log = new LogCon
        {
            text = $"{turnCnt}턴 시작"
        };

        DebugLogHandler.AddLog(LogType.OnlyText, log);

        // 현재 턴에 걸려있는 적의 cc기와 플레이어의 cc기를 하나 줄여준다.
        //battleEvent.InitNextSkill();

        StartCoroutine(battleEvent.ActionEvent(EventTime.StartTurn));

        if (turnCnt > 1)
        {
            ccHandler.DecreaseCC();
            fieldHandler.DecreaseTurn();
        }

        canPause = true;

        StartTurn();
    }

    public void StartTurn()
    {
        // 전부 돌려버리고
        mainRullet.RollRullet();

        // 멈추게 하는 버튼 활성화
        SetInteract(true);
    }

    public void SetPause(bool isPause)
    {
        if (canPause)
        {
            if (isPause)
            {
                mainRullet.PauseRullet();
                SetInteract(false);
            }
            else
            {
                StartTurn();
            }
        }
    }

    public void SetInteract(bool enable)
    {
        // 멈추게 하는 버튼 활성화
        stopHandler.SetInteract(enable);

        // 스크롤 버튼 활성화
        battleScroll.SetInteract(enable);
    }

    // 실행이 전부 끝나면 실행되는 코루틴
    private IEnumerator EndTurn()
    {
        yield return StartCoroutine(battleEvent.ActionEvent(EventTimeSkill.AfterSkill, result));
        yield return StartCoroutine(battleEvent.ActionEvent(EventTime.EndOfTurn));
        yield return pOneSecWait;

        // 기절 체크
        ccHandler.CheckCC(CCType.Stun);
        // 상처 체크
        ccHandler.CheckCC(CCType.Wound);

        CheckBattleEnd(() =>
        {
            battleUtil.SetPieceToInventory(result);
        });

        yield return null;

        // 저장한 결과를 인벤토리에 넣는다
        battleUtil.SetPieceToGraveyard(result);

        // 룰렛 조각 변경 (덱순환)
        yield return StartCoroutine(battleUtil.DrawRulletPieces());

        // 패널티 체크
        yield return StartCoroutine(CheckPanelty());

        yield return pOneSecWait;

        // 다음턴으로
        InitTurn();
    }

    public bool CheckBattleEnd(Action onEndBattle = null)
    {
        if (battleUtil.CheckDie(player))
        {
            onEndBattle?.Invoke();

            BattleEnd(false);

            StopAllCoroutines();

            return true;
        }
        else if (battleUtil.CheckEnemyDie(enemys))
        {
            onEndBattle?.Invoke();

            BattleEnd();

            StopAllCoroutines();

            return true;
        }

        return false;
    }

    public IEnumerator CheckPanelty(Action onEndCheckPanelty = null)
    {
        for (int i = 0; i < boolList.Count; i++)
        {
            while (battleUtil.CheckRulletPenalty(boolList[i]))
            {
                yield return pOneSecWait;
                yield return pOneSecWait;

                GivePenalty(boolList[i]);
                battleUtil.SetSkillsToGraveyard();

                if (CheckBattleEnd())
                {
                    yield break;
                }

                yield return null;

                yield return StartCoroutine(battleUtil.ResetRulletPiecesWithCondition(pos =>
                {
                    GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.15f, 0.1f);

                    GameManager.Instance.animHandler.GetAnim(AnimName.M_Butt)
                            .SetPosition(pos)
                            .SetScale(0.8f)
                            .Play();
                }));

                yield return null;

                yield return StartCoroutine(battleUtil.DrawRulletPieces());
            }
        }

        onEndCheckPanelty?.Invoke();

        yield break;
    }

    // 전투가 끝날 때
    private void BattleEnd(bool isWin = true)
    {
        canPause = false;
        isBattle = false;
        isBattleStart = false;

        turnCnt = 0;

        player.cc.ResetAllCC();
        player.RemoveShield();

        mainRullet.ResetRulletSpeed();
        battleEvent.ResetAllEvents();
        fieldHandler.SetFieldType(ElementalType.None);

        battleScroll.ShowScrollUI(open: false);
        GameManager.Instance.goldUIHandler.ShowGoldUI(open: false);

        LogCon log = new()
        {
            text = "전투 종료",
            hasLine = true
        };

        DebugLogHandler.AddLog(LogType.OnlyText, log);

        if (isWin)
        {
            enemys.Clear();
            if (isBoss && GameManager.Instance.IsEndStage())
            {
                print("리셋");
                GameManager.Instance.StageIdx = 0;
                GameManager.Instance.tbcHandler.StartEvent(() => GameManager.Instance.LoadScene(0), "메인화면으로 돌아갑니다");
            }
            else
            {
                battleRewardHandler.GiveReward();
            }
        }
        else
        {
            battleRewardHandler.ResetRullet(() =>
            {
                for (int i = 0; i < enemys.Count; i++)
                {
                    enemys[i].gameObject.SetActive(false);
                }
                enemys.Clear();
                GameManager.Instance.ResetGame();
                player.Init();
            });
        }
    }

    public void BattleForceEnd()
    {
        for (int i = enemys.Count - 1; i >= 0; i--)
        {
            enemys[i].Kill();
        }
    }

    #endregion

    #region Rullet Func

    // 결과 보여주기
    public void CastPiece(SkillPiece piece)
    {
        canPause = false;
        result = piece;

        if (piece != null)
        {
            // 침묵되어 있다면
            if (piece.Owner.GetComponent<LivingEntity>().cc.ccDic[CCType.Silence] > 0)
            {
                GameManager.Instance.animHandler.GetTextAnim()
                .SetType(TextUpAnimType.Up)
                .SetPosition(piece.skillImg.transform.position)
                .Play("침묵됨!");

                StartCoroutine(EndTurn());
                return;
            }

            StartCoroutine(battleEvent.ActionEvent(EventTimeSkill.WithSkill, piece));

            Action onShowCast = () => { };

            // 플레이어 스킬이라면
            if (piece.isPlayerSkill)
            {
                // 적이 한명 이하라면           조각이 대상 지정이 아니라면
                if (enemys.Count <= 1 || !piece.isTargeting)
                {
                    onShowCast = () =>
                    {
                        piece.Cast(enemys[0], () =>
                        {
                            StartCoroutine(EndTurn());
                        });

                        castUIHandler.EndCast(piece);
                    };
                }
                else
                {
                    battleTargetSelector.SelectTarget(target =>
                    {
                        piece.Cast(target, () =>
                        {
                            StartCoroutine(EndTurn());
                        });

                        castUIHandler.EndCast(piece);
                    });
                }

                mainRullet.RulletSpeed += 100f;
            }
            else
            {
                onShowCast = () =>
                {
                    piece.Cast(player, () =>
                    {
                        StartCoroutine(EndTurn());
                    });

                    castUIHandler.EndCast(piece);
                };

                mainRullet.RulletSpeed -= 200f;
            }

            castUIHandler.ShowCasting(piece, onShowCast);
        }
    }

    private void GivePenalty(bool isEnemy)
    {
        if (isEnemy)
        {
            for (int i = enemys.Count - 1; i >= 0; i--)
            {
                enemys[i].GetDamage(200);
            }
        }
        else
        {
            player.GetDamage((int)(player.maxHp * 0.3f));
        }

        GameManager.Instance.cameraHandler.ShakeCamera(1f, 0.2f);
    }
    #endregion
}
