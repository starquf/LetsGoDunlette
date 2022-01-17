using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHandler : MonoBehaviour
{
    // BattleHandler _ TypeC

    // 적의 스킬이 내 인벤토리에 들어가게 된다
    // 인벤토리에서 랜덤한 6개의 조각으로 채워진다 (단, 무조건 1개 이상의 적 조각과 2개 이상의 플레이어 조각이 들어가야한다)
    // 스킬 룰렛을 돌린다

    // 결과가 나올 때까지 기다린다 or 리롤을 한다
    // 결과가 나왔다면 나온 스킬이 발동된다

    // 공격이 전부 끝났다면 다시 룰렛을 돌린다.

    //==================================================

    [Header("메인 룰렛 멈추는 핸들러")]
    public StopSliderHandler stopHandler;

    private BattleInfoHandler battleInfoHandler;
    private CCHandler ccHandler;
    private BattleRewardHandler battleRewardHandler;

    private InventoryHandler inventory;

    //==================================================

    private BattleInfo battleInfo;

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
    public RulletPiece result;
    private int resultIdx;

    //==================================================

    [Header("플레이어&적 Health")]
    public PlayerHealth player;
    public EnemyHealth enemy;

    private EnemyInventory enemyInventory;

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
        ccList.Add(enemy.cc);

        ccHandler.Init(ccList);
        battleRewardHandler.Init(battleInfo.rewards);
    }

    private void CreateEnemy(List<EnemyHealth> enemyInfos)
    {
        enemy = Instantiate(enemyInfos[0]);
        enemyInventory = enemy.GetComponent<EnemyInventory>();
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

            this.result = result;
            resultIdx = pieceIdx;
        });
    }

    #endregion

    #region Init Rullet

    private IEnumerator InitRullet()
    {
        yield return null;

        // 적의 스킬을 추가해준다
        yield return new WaitForSeconds(enemyInventory.CreateSkillsSmooth() + 0.5f);

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

        // 잠시 기다리고
        yield return oneSecWait;

        // 적이 죽었는가?
        if (enemy.IsDie)
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

    // 전투가 끝날 때
    private void BattleEnd()
    {
        battleRewardHandler.GiveReward();
    }

    #endregion

    #region Rullet Func

    // 결과 보여주기
    private void CastResult()
    {
        if (result != null)
        {
            result.Cast(() =>
            {
                StartCoroutine(EndTurn());
            });
        }
        else
        {

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
