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
    private BattleScrollHandler battleScrollHandler;
    [HideInInspector]
    public BattleUtilHandler battleUtil;

    private InventoryHandler inventory;

    //==================================================

    private BattleInfo battleInfo;

    [Header("적을 생성하는 위치")]
    public Transform createTrans;

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

    [HideInInspector]
    public List<EnemyHealth> enemys = new List<EnemyHealth>();

    //==================================================
    
    private int turnCnt = 0;

    //==================================================

    public event Action<RulletPiece> onNextAttack;
    private event Action<RulletPiece> nextAttack;

    //==================================================

    #region WaitSeconds
    private readonly WaitForSeconds oneSecWait = new WaitForSeconds(1f);
    private readonly WaitForSeconds pFiveSecWait = new WaitForSeconds(0.5f);
    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);
    #endregion

    private void Awake()
    {
        GameManager.Instance.battleHandler = this;

        battleInfoHandler = GetComponent<BattleInfoHandler>();
        ccHandler = GetComponent<CCHandler>();
        battleRewardHandler = GetComponent<BattleRewardHandler>();
        battleTargetSelector = GetComponent<BattleTargetSelectHandler>();
        battleUtil = GetComponent<BattleUtilHandler>();
        battleScrollHandler = GetComponent<BattleScrollHandler>();
    }

    private void Start()
    {
        inventory = GameManager.Instance.inventoryHandler;

        // 플레이어가 가지고 있는 기본 스킬 생성 일단 테스트로 만들어놈
        player.GetComponent<Inventory>().CreateSkills();
    }

    #region StartBattle

    // 전투를 시작하는 함수
    public void StartBattle()
    {
        print("전투시작");
        onNextAttack = null;
        nextAttack = null;

        // 현재 전투 정보 가져오기
        battleInfo = battleInfoHandler.GetRandomBattleInfo();

        // 적 생성 일단 테스트로 하나만 만듬
        CreateEnemy(battleInfo.enemyInfos);

        // 핸들러들 초기화
        InitHandler();

        // 스탑 버튼에 기능 추가
        SetStopHandler();

        // 전투가 시작하기 전 인벤토리와 룰렛 정리
        StartCoroutine(InitRullet());
    }

    private void InitHandler()
    {
        List<CrowdControl> ccList = new List<CrowdControl>();
        ccList.Add(player.cc);

        for (int i = 0; i < enemys.Count; i++)
        {
            ccList.Add(enemys[i].cc);
        }

        ccHandler.Init(ccList);
        battleRewardHandler.Init(battleInfo.rewards);
        battleUtil.Init(inventory, mainRullet);
    }

    public void CreateEnemy(List<EnemyHealth> enemyInfos) //다중생성
    {
        for (int i = 0; i < enemyInfos.Count; i++)
        {
            EnemyHealth enemy = Instantiate(enemyInfos[i]);
            enemy.transform.position = createTrans.position;

            enemys.Add(enemy);
        }

        // 보스면 가운데와 바꿔줘야된다 
        SortBoss();
        SetEnemyPosition();
    }

    public void CreateEnemy(EnemyHealth enemyInfo) //단일생성
    {
        EnemyHealth enemy = Instantiate(enemyInfo);
        enemy.transform.position = createTrans.position;

        enemys.Add(enemy);

        // 보스면 가운데와 바꿔줘야된다 
        SortBoss();
        SetEnemyPosition();
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
        if (enemys.Count <= 0) return;

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
            battleScrollHandler.SetInteract(false);
        });
    }

    #endregion

    #region Init Rullet

    private IEnumerator InitRullet()
    {
        yield return null;

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

        yield return new WaitForSeconds(maxTime + 0.5f);

        // 인벤토리에서 랜덤한 6개의 스킬을 뽑아 룰렛에 적용한다. 단, 최소한 적의 스킬 1개와 내 스킬 2개가 보장된다.

        // 플레이어 확정 2개
        for (int i = 0; i < 2; i++)
        {
            SetRandomPlayerOrEnemySkill(true);
            yield return new WaitForSeconds(0.15f);
        }

        // 적 확정 하나
        SetRandomPlayerOrEnemySkill(false);
        yield return new WaitForSeconds(0.15f);

        // 나머지 랜덤 3개
        for (int i = 0; i < 3; i++)
        {
            SetRandomSkill();

            yield return new WaitForSeconds(0.15f);
        }

        yield return oneSecWait;

        // 턴 시작
        InitTurn();
    }

    private void SetRandomPlayerOrEnemySkill(bool isPlayer)
    {
        SkillPiece skill = inventory.GetRandomPlayerOrEnemySkill(isPlayer);
        skill.transform.localScale = new Vector3(0.2f, 0.2f, 1f);

        mainRullet.AddPiece(skill);
    }

    private void SetRandomSkill()
    {
        SkillPiece skill = inventory.GetRandomUnusedSkill();
        skill.transform.localScale = new Vector3(0.2f, 0.2f, 1f);

        mainRullet.AddPiece(skill);
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

        // 현재 턴에 걸려있는 적의 cc기와 플레이어의 cc기를 하나 줄여준다.
        ccHandler.DecreaseCC();

        nextAttack = null;
        nextAttack = onNextAttack;

        StartTurn();
    }

    public void StartTurn()
    {
        // 전부 돌려버리고
        mainRullet.RollRullet();

        // 멈추게 하는 버튼 활성화
        stopHandler.SetInteract(true);

        // 스크롤 버튼 활성화
        battleScrollHandler.SetInteract(true);
    }

    // 실행이 전부 끝나면 실행되는 코루틴
    private IEnumerator EndTurn()
    {
        // 다음 공격 체크하는 스킬들이 발동되는 타이밍
        nextAttack?.Invoke(result);

        yield return pOneSecWait;

        // 기절 체크
        ccHandler.CheckCC(CCType.Stun);
        // 상처 체크
        ccHandler.CheckCC(CCType.Wound);

        // 적이 전부 죽었는가?
        if (CheckEnemyDie())
        {
            battleUtil.SetPieceToInventory(result);

            BattleEnd();
            yield break;
        }
        else if(CheckPlayerDie())
        {
            battleUtil.SetPieceToInventory(result);

            BattleEnd(false);
            yield break;
        }

        yield return null;

        // 저장한 결과를 인벤토리에 넣는다
        battleUtil.SetPieceToGraveyard(result);

        // 룰렛 조각 변경 (덱순환)
        battleUtil.DrawRulletPieces();

        // 패널티 체크
        if (battleUtil.CheckRulletPenalty(false))
        {
            yield return pFiveSecWait;

            GivePenalty(false);
            yield return null;
            battleUtil.ResetRullet();

            if (CheckPlayerDie())
            {
                BattleEnd(false);
                yield break;
            }
        }
        else if (battleUtil.CheckRulletPenalty(true))
        {
            yield return pFiveSecWait;

            GivePenalty(true);
            yield return null;
            battleUtil.ResetRullet();

            if (CheckEnemyDie())
            {
                BattleEnd();
                yield break;
            }
        }

        yield return pFiveSecWait;

        // 다음턴으로
        InitTurn();
    }

    private bool CheckPlayerDie()
    {
        return player.IsDie;
    }

    private bool CheckEnemyDie()
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            if (!enemys[i].IsDie)
            {
                return false;
            }
        }

        return true;
    }

    // 전투가 끝날 때
    private void BattleEnd(bool isWin = true)
    {
        player.cc.ResetAllCC();
        mainRullet.speedWeight = 0f;

        if (isWin)
        {
            enemys.Clear();
            battleRewardHandler.GiveReward();
        }
        else
        {
            battleRewardHandler.ResetRullet(()=> {
                for (int i = 0; i < enemys.Count; i++)
                {
                    enemys[i].gameObject.SetActive(false);
                }
                enemys.Clear();
                GameManager.Instance.mapHandler.GameOverProto();
                player.Revive();
            });
        }
    }

    #endregion

    #region Rullet Func

    // 결과 보여주기
    public void CastPiece(SkillPiece piece)
    {
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

            Action onShowCast = () => { };

            // 플레이어 스킬이라면
            if (piece.isPlayerSkill)
            {
                battleTargetSelector.SelectTarget(target => {
                    piece.Cast(target, () =>
                    {
                        StartCoroutine(EndTurn());
                    });

                    castUIHandler.EndCast(piece);
                });

                mainRullet.speedWeight += 50f;
            }
            else
            {
                onShowCast = () =>
                {
                    piece.Cast(player, () =>
                    {
                        castUIHandler.EndCast(piece);
                        StartCoroutine(EndTurn());
                    });
                };

                mainRullet.speedWeight = 0f;
            }

            castUIHandler.ShowCasting(piece, onShowCast);
        }
    }

    private void GivePenalty(bool isEnemy)
    {
        if (isEnemy)
        {
            for (int i = 0; i < enemys.Count; i++)
            {
                enemys[i].GetDamage(100);
            }
        }
        else
        {
            player.GetDamage(100);
        }

        GameManager.Instance.cameraHandler.ShakeCamera(1f, 0.2f);
    }
    #endregion
}
