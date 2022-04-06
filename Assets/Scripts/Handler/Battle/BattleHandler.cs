using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<EnemyHealth> enemys = new List<EnemyHealth>();

    //==================================================

    private int turnCnt = 0;

    //==================================================

    public Transform bottomPos;

    //==================================================

    [Header("현재 맵 보스 타입")]
    public BattleInfo _bInfo = null;
    public bool isElite = false;
    public bool isBoss = false;

    [Header("UI들")]
    public BottomUIHandler bottomUI;

    #region WaitSeconds
    private readonly WaitForSeconds oneSecWait = new WaitForSeconds(1f);
    private readonly WaitForSeconds pFiveSecWait = new WaitForSeconds(0.5f);
    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);
    private List<bool> boolList = new List<bool>() { true, false };
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
            if (_bInfo == null)
            {
                Debug.LogError("보스 설정이 안되어있음");
                return;
            }
            battleInfo = _bInfo;
            // 랜덤 전투 정보 가져오기
            //battleInfo = battleInfoHandler.GetRandomBossInfo();
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

        // 적 생성
        CreateEnemy(battleInfo.enemyInfos, () =>
        {
            // 전투가 시작하기 전 인벤토리와 룰렛 정리
            StartCoroutine(InitRullet());
        });

        // 핸들러들 초기화
        InitHandler();

        // 스탑 버튼에 기능 추가
        SetStopHandler();

        LogCon log = new LogCon();
        log.text = "전투 시작";
        log.hasLine = true;

        DebugLogHandler.AddLog(LogType.OnlyText, log);

        battleEvent.OnStartBattle();
    }

    private void InitHandler()
    {
        battleRewardHandler.Init(GameManager.Instance.skillContainer.playerSkillPrefabs);
        battleUtil.Init(inventory, mainRullet);
    }

    public int SetRandomBoss()
    {
        _bInfo = battleInfoHandler.GetRandomBossInfo();
        switch (_bInfo.enemyInfos[0])
        {
            case EnemyType.QUEEN:
                return 0;
            case EnemyType.REDFOX:
                return 1;
            default:
                Debug.LogError("이상한 보스가 설정됨");
                return -1;
        }
    }

    public void CreateEnemy(List<EnemyType> enemyInfos, Action onCreateEnd) //다중생성
    {
        List<EnemyHealth> createdEnemy = new List<EnemyHealth>();

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
                EnemyHealth temp = enemys[idx];

                enemys[idx] = enemys[i];
                enemys[i] = temp;
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

        Vector2 screenX = new Vector2(Camera.main.ViewportToWorldPoint(Vector3.zero).x, Camera.main.ViewportToWorldPoint(Vector3.one).x);
        float posX = (Mathf.Abs(screenX.x) + screenX.y) / (float)(enemyCount + 1);

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
        HideEnemyIndicator();

        // 버튼 초기화
        stopHandler.SetInteract(false);

        result = null;

        turnCnt++;

        LogCon log = new LogCon();
        log.text = "";
        log.hasLine = true;

        DebugLogHandler.AddLog(LogType.OnlyText, log);

        log = new LogCon();
        log.text = $"{turnCnt}턴 시작";

        DebugLogHandler.AddLog(LogType.OnlyText, log);

        // 현재 턴에 걸려있는 적의 cc기와 플레이어의 cc기를 하나 줄여준다.
        battleEvent.InitNextSkill();
        battleEvent.OnStartTurn();

        if (turnCnt > 1)
        {
            ccHandler.DecreaseCC();
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
        battleEvent.OnEndTurn();

        // 다음 공격 체크하는 스킬들이 발동되는 타이밍
        battleEvent.OnNextSkill(result);

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

                yield return null;

                yield return StartCoroutine(battleUtil.ResetRulletPiecesWithCondition(pos =>
                {
                    GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.15f, 0.1f);

                    Anim_M_Butt hitEffect = PoolManager.GetItem<Anim_M_Butt>();
                    hitEffect.SetScale(0.8f);
                    hitEffect.transform.position = pos;

                    hitEffect.Play();
                }));

                if (CheckBattleEnd())
                {
                    yield break;
                }

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

        battleScroll.ShowScrollUI(open: false);
        GameManager.Instance.goldUIHandler.ShowGoldUI(open: false);

        LogCon log = new LogCon();
        log.text = "전투 종료";
        log.hasLine = true;

        DebugLogHandler.AddLog(LogType.OnlyText, log);

        if (isWin)
        {
            enemys.Clear();
            battleRewardHandler.GiveReward();
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
                GameManager.Instance.mapHandler.GameOverProto();
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
            if (piece.owner.GetComponent<LivingEntity>().cc.ccDic[CCType.Silence] > 0)
            {
                Anim_TextUp silenceTextEffect = PoolManager.GetItem<Anim_TextUp>();
                silenceTextEffect.SetType(TextUpAnimType.Damage);
                silenceTextEffect.transform.position = piece.skillImg.transform.position;
                silenceTextEffect.Play("침묵됨!");

                StartCoroutine(EndTurn());
                return;
            }

            battleEvent.OnCastPiece(piece);

            Action onShowCast = () => { };

            // 플레이어 스킬이라면
            if (piece.isPlayerSkill)
            {
                // 적이 한명 이하라면           조각이 대상 지정이 아니라면
                if (enemys.Count <= 1 || !piece.hasTarget)
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
