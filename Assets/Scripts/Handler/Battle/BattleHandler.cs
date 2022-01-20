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

    private InventoryHandler inventory;

    //==================================================

    private BattleInfo battleInfo;

    [Header("적을 생성하는 위치")]
    public Transform createTrans;

    [Header("적 공통")]
    public Transform hpBar;
    public Transform hpShieldBar;
    public Text hpText;
    public Transform damageTrans;
    public Transform ccUITrm;

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
    #endregion

    private void Awake()
    {
        GameManager.Instance.battleHandler = this;

        battleInfoHandler = GetComponent<BattleInfoHandler>();
        ccHandler = GetComponent<CCHandler>();
        battleRewardHandler = GetComponent<BattleRewardHandler>();
        battleTargetSelector = GetComponent<BattleTargetSelectHandler>();
    }

    private void Start()
    {
        inventory = GameManager.Instance.inventoryHandler;

        // 플레이어가 가지고 있는 기본 스킬 생성 일단 테스트로 만들어놈
        player.GetComponent<Inventory>().CreateSkills();

        //StartBattle();
    }

    #region StartBattle

    // 전투를 시작하는 함수
    public void StartBattle()
    {
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
    }

    private void CreateEnemy(List<EnemyHealth> enemyInfos)
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

        // 버튼 초기화
        stopHandler.SetInteract(false);

        result = null;

        turnCnt++;

        // 현재 턴에 걸려있는 적의 cc기와 플레이어의 cc기를 하나 줄여준다.
        ccHandler.DecreaseCC();

        nextAttack = null;
        nextAttack = onNextAttack;

        // 턴 시작 로직
        StartCoroutine(CheckTurn());
    }

    private IEnumerator CheckTurn()
    {
        // 전부 돌려버리고
        RollAllRullet();

        // 멈추게 하는 버튼 활성화
        stopHandler.SetInteract(true);

        // 전부 돌릴 때까지 기다린다
        yield return new WaitUntil(CheckRullet);

        // 결과 보여주고
        yield return oneSecWait;

        // 결과를 저장해놓고 그 칸을 빈칸으로 만들어준다
        SetRulletEmpty(resultIdx);

        // 결과 실행
        CastResult();
    }

    // 실행이 전부 끝나면 실행되는 코루틴
    private IEnumerator EndTurn()
    {
        // 다음 공격 체크하는 스킬들이 발동되는 타이밍
        nextAttack?.Invoke(result);

        yield return pFiveSecWait;

        // 저장한 결과를 인벤토리에 넣는다
        SetPieceToGraveyard(result as SkillPiece);

        // 기절 체크
        ccHandler.CheckCC(CCType.Stun);
        // 상처 체크
        ccHandler.CheckCC(CCType.Wound);

        // 잠시 기다리고
        yield return oneSecWait;

        // 적이 전부 죽었는가?
        if (CheckEnemyDie())
        {
            BattleEnd();
            yield break;
        }

        yield return pFiveSecWait;

        // 룰렛 조각 변경 (덱순환)
        DrawRulletPieces();

        yield return pFiveSecWait;

        // 다음턴으로
        InitTurn();
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
    private void BattleEnd()
    {
        enemys.Clear();
        battleRewardHandler.GiveReward();
    }

    #endregion

    #region Rullet Func

    // 결과 보여주기
    private void CastResult()
    {
        if (result != null)
        {
            Action onShowCast = () => { };

            // 플레이어 스킬이라면
            if (result.isPlayerSkill)
            {
                battleTargetSelector.SelectTarget(target => {
                    result.Cast(target, () =>
                    {
                        castUIHandler.ShowPanel(false);
                        StartCoroutine(EndTurn());
                    });
                });
            }
            else
            {
                onShowCast = () =>
                {
                    result.Cast(player, () =>
                    {
                        castUIHandler.ShowPanel(false);
                        StartCoroutine(EndTurn());
                    });
                };
            }

            castUIHandler.ShowCasting(result, onShowCast);
        }
    }

    private bool CheckRullet()
    {
        if (mainRullet.IsRoll)
        {
            return false;
        }

        return true;
    }

    public void ChangeRulletPiece(int pieceIdx)
    {
        SkillPiece skill = inventory.GetRandomUnusedSkill();

        mainRullet.GetComponent<SkillRullet>().ChangePiece(pieceIdx, skill);
    }

    public void DrawRulletPieces()
    {
        List<RulletPiece> pieces = mainRullet.GetPieces();

        for (int i = 0; i < pieces.Count; i++)
        {
            // 비어있는곳이라면
            if (pieces[i] == null)
            {
                SkillPiece skill = inventory.GetRandomUnusedSkill();
                mainRullet.SetPiece(i, skill);
            }
        }
    }

    public void SetPieceToGraveyard(int pieceIdx)
    {
        mainRullet.PutRulletPieceToGraveYard(pieceIdx);
    }

    public void SetPieceToGraveyard(SkillPiece piece)
    {
        GameManager.Instance.inventoryHandler.SetUseSkill(piece);
    }

    public void SetRulletEmpty(int pieceIdx)
    {
        mainRullet.SetEmpty(pieceIdx);
    }

    private void RollAllRullet()
    {
        mainRullet.RollRullet();
    }

    #endregion
}
