using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class BattleHandler : MonoBehaviour
{
    // BattleHandler _ TypeC

    // 적의 스킬이 내 인벤토리에 들어가게 된다
    // 인벤토리에서 랜덤한 6개의 조각으로 채워진다 (단, 무조건 1개 이상의 적 조각과 2개 이상의 플레이어 조각이 들어가야한다)
    // 스킬 룰렛을 돌린다

    // 결과가 나올 때까지 기다린다 or 리롤을 한다
    // 결과가 나왔다면 나온 스킬이 발동된다

    // 공격이 전부 끝났다면 다시 룰렛을 돌린다.

    // 탭
    public CanvasGroup tapGroup;
    public Color tapColor;

    private InventoryHandler inventory;

    //==================================================

    [Header("룰렛들")]

    // 메인, 서브 or 나중에 추가될지도 모르는 룰렛
    public List<Rullet> rullets = new List<Rullet>();

    // 결과로 나온 룰렛조각들
    public RulletPiece result;
    private int resultIdx;

    //==================================================

    [Header("플레이어&적 Health")]
    public PlayerHealth player;
    public EnemyHealth enemy;

    private EnemyAttack enemyAtk;
    private EnemyReward enemyReward;

    //==================================================

    private bool isTap = false;
    public bool IsTap => isTap;

    private int rerollGold = 5;
    private bool canReroll = false;

    private int turnCnt = 0;

    //==================================================

    public event Action<RulletPiece> onNextAttack;
    private event Action<RulletPiece> nextAttack;

    public bool IsContract { get; set; }
    public int ContractDmg { get; private set; }
    public int ContractRemain { get; private set; }

    //==================================================

    private Tween blinkTween;

    #region WaitSeconds
    private readonly WaitForSeconds oneSecWait = new WaitForSeconds(1f);
    private readonly WaitForSeconds pFiveSecWait = new WaitForSeconds(0.5f);
    #endregion

    private void Awake()
    {
        GameManager.Instance.battleHandler = this;

        enemyAtk = enemy.GetComponent<EnemyAttack>();
        enemyReward = enemy.GetComponent<EnemyReward>();
    }

    private void Start()
    {
        inventory = GameManager.Instance.inventoryHandler;

        // 리롤 & 스탑 버튼 추가
        tapGroup.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (!isTap)
            {
                StopPlayerRullet();
            }
            else if (canReroll)
            {
                ReRoll();
            }
        });
        tapGroup.interactable = false;

        // 전투가 시작하기 전 인벤토리와 룰렛 정리
        StartCoroutine(InitRullet());
    }

    private void SetRandomPlayerOrEnemySkill(bool isPlayer)
    {
        SkillPiece skill = inventory.GetRandomPlayerOrEnemySkill(isPlayer);
        skill.transform.localScale = new Vector3(0.2f, 0.2f, 1f);

        rullets[0].AddPiece(skill);
    }

    private void SetRandomSkill()
    {
        SkillPiece skill = inventory.GetRandomUnusedSkill();
        skill.transform.localScale = new Vector3(0.2f, 0.2f, 1f);

        rullets[0].AddPiece(skill);
    }

    private IEnumerator InitRullet()
    {
        // 적의 스킬을 추가해준다
        yield return new WaitForSeconds(enemyAtk.AddAllSkills() + 0.5f);

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

    // 다음 턴으로 넘어가는 것
    private void InitTurn()
    {
        // 버튼 초기화
        isTap = false;
        tapGroup.interactable = false;

        result = null;

        turnCnt++;

        // 계약 체크
        CheckContract();

        // 현재 턴에 걸려있는 적의 cc기와 플레이어의 cc기를 하나 줄여준다.
        DecreaseCC();

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
        tapGroup.interactable = true;

        // 전부 돌릴 때까지
        yield return new WaitUntil(CheckRullet);

        // 리롤 더이상 못함
        canReroll = false;

        blinkTween.Kill();
        tapGroup.GetComponent<Image>().color = Color.black;

        // 결과 보여주고
        yield return oneSecWait;

        // 결과를 저장해놓고 그 칸을 null로 만들어준다
        ExeptRulletResult(resultIdx);

        // 결과 실행
        CastResult();
    }

    // 실행이 전부 끝나면 실행되는 코루틴
    private IEnumerator EndTurn()
    {
        // 룰렛 리셋은 인벤토리가 알아서 해줌
        yield return pFiveSecWait;

        // 저장한 결과를 인벤토리에 넣는다
        SetUseRulletPiece(result as SkillPiece);

        // 기절이라면 조각을 날려버린다
        CheckStun();

        // 잠시 기다리고
        yield return oneSecWait;

        // 적이 죽었는가?
        if (enemy.IsDie)
        {
            yield return new WaitUntil(() => !enemyReward.IsReward);

            //GoNextRoom();
            yield return oneSecWait;
            yield return oneSecWait;

            enemy.Revive();

            yield return new WaitUntil(() => !enemy.IsDie);
        }

        yield return pFiveSecWait;

        // 룰렛 조각 변경 (덱순환)
        DrawRulletPieces();

        yield return pFiveSecWait;

        tapGroup.GetComponent<Image>().DOColor(tapColor, 0.2f);
        tapGroup.transform.GetChild(0).transform.DOLocalMoveY(0f, 0.2f);

        // 다음턴으로
        InitTurn();
    }

    // 모든 룰렛을 멈추게 하는 함수
    private void StopPlayerRullet()
    {
        for (int i = 0; i < rullets.Count; i++)
        {
            int a = i;

            rullets[i].StopRullet((result, pieceIdx) =>
            {
                rullets[a].HighlightResult();

                //ComboManager.Instance.AddComboQueue(result);
                this.result = result;

                resultIdx = pieceIdx;
            });
        }

        SetReroll();
    }

    // 리롤 기능을 켜주게 하는 함수
    private void SetReroll()
    {
        isTap = true;
        canReroll = true;

        rerollGold = 5;
        tapGroup.transform.GetChild(0).transform.DOLocalMoveY(-180f, 0.2f);

        blinkTween = tapGroup.GetComponent<Image>().DOColor(Color.black, 1f)
            .SetEase(Ease.Flash, 20, 0)
            .SetLoops(-1);
    }

    // 결과 보여주기
    private void CastResult()
    {
        if (result != null)
        {
            result.Cast(() => 
            {
                StartCoroutine(EndTurn());
            });

            nextAttack?.Invoke(result);
        }
        else
        {
            
        }
    }

    private void ReRoll()
    {
        GameManager.Instance.Gold -= rerollGold;

        rerollGold += 5;

        if (GameManager.Instance.Gold <= rerollGold)
        {
            blinkTween.Kill();
            tapGroup.GetComponent<Image>().color = Color.black;

            canReroll = false;
        }

        for (int i = 0; i < rullets.Count; i++)
        {
            rullets[i].ReRoll();
        }
    }

    private bool CheckRullet()
    {
        for (int i = 0; i < rullets.Count; i++)
        {
            if (rullets[i].IsRoll)
            {
                return false;
            }
        }

        return true;
    }

    public void ChangeRulletPiece(int pieceIdx)
    {
        SkillPiece skill = inventory.GetRandomUnusedSkill();
        //GameManager.Instance.inventorySystem.EquiqedSkills.Add(skill);
        //skill.state = PieceState.EQUIQED;

        rullets[0].GetComponent<SkillRullet>().ChangePiece(pieceIdx, skill);
    }

    public void DrawRulletPieces()
    {
        SkillRullet rullet = rullets[0].GetComponent<SkillRullet>();

        List<RulletPiece> pieces = rullet.GetPieces();

        for (int i = 0; i < pieces.Count; i++)
        {
            // 비어있는곳이라면
            if (pieces[i] == null)
            {
                SkillPiece skill = inventory.GetRandomUnusedSkill();
                rullet.SetPiece(i, skill);
            }
        }
    }

    public void SetUseRulletPiece(int pieceIdx)
    {
        rullets[0].GetComponent<SkillRullet>().SetUsePiece(pieceIdx);
    }

    public void SetUseRulletPiece(SkillPiece piece)
    {
        GameManager.Instance.inventoryHandler.SetUseSkill(piece);
    }

    public void ExeptRulletResult(int pieceIdx)
    {
        rullets[0].GetComponent<SkillRullet>().SetExeptPiece(pieceIdx);
    }

    private void RollAllRullet()
    {
        for (int i = 0; i < rullets.Count; i++)
        {
            int a = i;

            rullets[i].RollRullet();
        }
    }

    public void SetContract(int contractDmg, int contractRemain = 3)
    {
        IsContract = true;
        ContractDmg = contractDmg;
        ContractRemain = contractRemain;
    }

    private void CheckContract()
    {
        if (IsContract)
        {
            ContractRemain--;

            if (ContractRemain <= 0)
            {
                IsContract = false;
            }
        }
    }

    private void DecreaseCC()
    {
        player.cc.DecreaseAllTurn();
        enemy.cc.DecreaseAllTurn();
    }

    private void CheckStun()
    {
        // 기절되어있다면
        if (player.cc.ccDic[CCType.Stun] > 0)
        {
            Stun(true);
        }

        if(enemy.cc.ccDic[CCType.Stun] > 0)
        {
            Stun(false);
        }
    }

    private void Stun(bool isPlayer)
    {
        List<RulletPiece> pieces = rullets[0].GetPieces();

        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i] == null) continue;

            // 플레이어의 조각이라면
            if ((pieces[i] as SkillPiece).isPlayerSkill.Equals(isPlayer))
            {
                (rullets[0] as SkillRullet).SetUsePiece(i);
            }
        }
    }
}
