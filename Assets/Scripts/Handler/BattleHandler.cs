using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class BattleHandler : MonoBehaviour
{
    // BattleHandler _ TypeC

    // ���� ��ų�� �� �κ��丮�� ���� �ȴ�
    // �κ��丮���� ������ 6���� �������� ä������ (��, ������ 1�� �̻��� �� ������ 2�� �̻��� �÷��̾� ������ �����Ѵ�)
    // ��ų �귿�� ������

    // ����� ���� ������ ��ٸ��� or ������ �Ѵ�
    // ����� ���Դٸ� ���� ��ų�� �ߵ��ȴ�

    // ������ ���� �����ٸ� �ٽ� �귿�� ������.

    // ��
    public CanvasGroup tapGroup;

    public Color tapColor;

    // �� �귿
    [Header("�귿��")]
    // ����, ���� or ���߿� �߰������� �𸣴� �귿
    public List<Rullet> rullets = new List<Rullet>();
    // ����� ���� �귿������
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

    // ���� ������ �Ѿ�� ��
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
        // ���� ����������
        RollAllRullet();

        // ���߰� �ϴ� ��ư Ȱ��ȭ
        tapGroup.interactable = true;

        // ���� ���� ������
        yield return new WaitUntil(CheckRullet);

        // ���� ���̻� ����
        canReroll = false;

        blinkTween.Kill();
        tapGroup.GetComponent<Image>().color = Color.black;

        // ��� �����ְ�
        yield return new WaitForSeconds(1f);

        // ��� ����
        CastResult();

        // ���� �����ְ�
        yield return new WaitForSeconds(1f);

        // �귿 ����
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

        // �귿 ���� ���� (����ȯ)
        ChangeRulletPiece(resultIdx);

        yield return new WaitForSeconds(0.5f);

        tapGroup.GetComponent<Image>().DOColor(tapColor, 0.2f);
        tapGroup.transform.GetChild(0).transform.DOLocalMoveY(0f, 0.2f);

        // ��ư �ʱ�ȭ
        isTap = false;
        tapGroup.interactable = false;

        // ����������
        InitTurn();
    }

    // ��� �����ֱ�
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
