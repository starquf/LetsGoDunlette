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

    // 턴 룰렛
    [Header("룰렛들")]
    // 메인, 서브 or 나중에 추가될지도 모르는 룰렛
    public List<Rullet> rullets = new List<Rullet>();
    // 결과로 나온 룰렛조각들
    public List<RulletPiece> results = new List<RulletPiece>();
    private int resultIdx;

    [Space(10f)]
    private InventoryHandler inventory;

    public PlayerHealth player;

    public EnemyHealth enemy;
    private EnemyAttack enemyAtk;
    private EnemyReward enemyReward;

    private bool isTap = false;
    public bool IsTap => isTap;

    private int rerollCnt = 3;
    private bool canReroll = false;

    private int turnCnt = 0;

    private Tween blinkTween;

    private void Awake()
    {
        GameManager.Instance.battleHandler = this;

        enemyAtk = enemy.GetComponent<EnemyAttack>();
        enemyReward = enemy.GetComponent<EnemyReward>();
    }

    private void Start()
    {
        inventory = GameManager.Instance.inventorySystem;

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

        StartCoroutine(SetRandomSkill());
    }

    private IEnumerator SetRandomSkill()
    {
        yield return new WaitForSeconds(enemyAtk.AddAllSkills() + 0.5f);

        for (int i = 0; i < 6; i++)
        {
            SkillPiece skill = inventory.GetRandomUnusedSkill();
            //skill.state = PieceState.EQUIQED;
            skill.transform.localScale = new Vector3(0.2f, 0.2f, 1f);

            rullets[0].AddPiece(skill);

            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(1f);

        InitTurn();
    }

    // 다음 턴으로 넘어가는 것
    private void InitTurn()
    {
        turnCnt++;

        StartCoroutine(CheckTurn());
    }

    private void StopPlayerRullet()
    {
        for (int i = 0; i < rullets.Count; i++)
        {
            int a = i;

            rullets[i].StopRullet((result, pieceIdx) =>
            {
                rullets[a].HighlightResult();

                //ComboManager.Instance.AddComboQueue(result);
                results.Add(result);

                resultIdx = pieceIdx;
            });
        }

        isTap = true;
        canReroll = true;

        rerollCnt = 3;
        tapGroup.transform.GetChild(0).transform.DOLocalMoveY(-180f, 0.2f);

        blinkTween = tapGroup.GetComponent<Image>().DOColor(Color.black, 1f)
            .SetEase(Ease.Flash, 20, 0)
            .SetLoops(-1);
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
        yield return new WaitForSeconds(1f);

        // 결과 실행
        CastResult();

        // 실행 보여주고
        yield return new WaitForSeconds(1f);

        // 룰렛 리셋
        ResetRullets();

        if (enemy.IsDie)
        {
            yield return new WaitUntil(() => !enemyReward.IsReward);

            //GoNextRoom();
            yield return new WaitForSeconds(2f);
            enemy.Revive();

            yield return new WaitUntil(() => !enemy.IsDie);
        }

        yield return new WaitForSeconds(0.5f);

        // 룰렛 조각 변경 (덱순환)
        ChangeRulletPiece(resultIdx);

        yield return new WaitForSeconds(0.5f);

        tapGroup.GetComponent<Image>().DOColor(tapColor, 0.2f);
        tapGroup.transform.GetChild(0).transform.DOLocalMoveY(0f, 0.2f);

        // 버튼 초기화
        isTap = false;
        tapGroup.interactable = false;

        // 다음턴으로
        InitTurn();
    }

    // 결과 보여주기
    private void CastResult()
    {
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i] != null)
            {
                results[i].Cast();
            }
            else
            {

            }
        }

        results.Clear();
    }

    private void ReRoll()
    {
        rerollCnt--;

        if (rerollCnt <= 0)
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

    private void ResetRullets()
    {
        for (int i = 0; i < rullets.Count; i++)
        {
            rullets[i].ResetRulletSize();
        }
    }

    private void ChangeRulletPiece(int pieceIdx)
    {
        SkillPiece skill = inventory.GetRandomUnusedSkill();
        //GameManager.Instance.inventorySystem.EquiqedSkills.Add(skill);
        //skill.state = PieceState.EQUIQED;

        rullets[0].GetComponent<SkillRullet>().ChangePiece(pieceIdx, skill);
    }

    private void RollAllRullet()
    {
        for (int i = 0; i < rullets.Count; i++)
        {
            int a = i;

            rullets[i].RollRullet();
        }
    }
}
