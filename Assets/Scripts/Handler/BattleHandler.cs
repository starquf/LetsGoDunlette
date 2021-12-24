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

    private InventoryHandler inventory;

    //==================================================

    [Header("�귿��")]

    // ����, ���� or ���߿� �߰������� �𸣴� �귿
    public List<Rullet> rullets = new List<Rullet>();

    // ����� ���� �귿������
    public RulletPiece result;
    private int resultIdx;

    //==================================================

    [Header("�÷��̾�&�� Health")]
    public PlayerHealth player;
    public EnemyHealth enemy;

    private EnemyAttack enemyAtk;
    private EnemyReward enemyReward;

    //==================================================

    private bool isTap = false;
    public bool IsTap => isTap;

    private int rerollCnt = 3;
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

        // ���� & ��ž ��ư �߰�
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

        // ������ �����ϱ� �� �κ��丮�� �귿 ����
        StartCoroutine(SetRandomSkill());
    }

    private IEnumerator SetRandomSkill()
    {
        // ���� ��ų�� �߰����ش�
        yield return new WaitForSeconds(enemyAtk.AddAllSkills() + 0.5f);

        // �κ��丮���� ������ 6���� ��ų�� �̾� �귿�� �����Ѵ�.
        for (int i = 0; i < 6; i++)
        {
            SkillPiece skill = inventory.GetRandomUnusedSkill();
            //skill.state = PieceState.EQUIQED;
            skill.transform.localScale = new Vector3(0.2f, 0.2f, 1f);

            rullets[0].AddPiece(skill);

            yield return new WaitForSeconds(0.15f);
        }

        yield return oneSecWait;

        // �� ����
        InitTurn();
    }

    // ���� ������ �Ѿ�� ��
    private void InitTurn()
    {
        turnCnt++;

        CheckContract();

        nextAttack = null;
        nextAttack = onNextAttack;

        // �� ���� ����
        StartCoroutine(CheckTurn());
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
        yield return oneSecWait;

        // �귿 ����
        ResetRullets();

        yield return pFiveSecWait;

        // ��� ����
        CastResult();

        // �κ��丮�� �ִ´�
        SetUseRulletPiece(resultIdx);
    }

    // ������ ���� ������ ����Ǵ� �ڷ�ƾ
    private IEnumerator EndTurn()
    {
        // ��� ��ٸ���
        yield return oneSecWait;

        // ���� �׾��°�?
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

        // �귿 ���� ���� (����ȯ)
        DrawRulletPieces();

        yield return pFiveSecWait;

        tapGroup.GetComponent<Image>().DOColor(tapColor, 0.2f);
        tapGroup.transform.GetChild(0).transform.DOLocalMoveY(0f, 0.2f);

        // ��ư �ʱ�ȭ
        isTap = false;
        tapGroup.interactable = false;

        // ����������
        InitTurn();
    }

    // ��� �귿�� ���߰� �ϴ� �Լ�
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

    // ���� ����� ���ְ� �ϴ� �Լ�
    private void SetReroll()
    {
        isTap = true;
        canReroll = true;

        rerollCnt = 3;
        tapGroup.transform.GetChild(0).transform.DOLocalMoveY(-180f, 0.2f);

        blinkTween = tapGroup.GetComponent<Image>().DOColor(Color.black, 1f)
            .SetEase(Ease.Flash, 20, 0)
            .SetLoops(-1);
    }

    // ��� �����ֱ�
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

        result = null;
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
            // ����ִ°��̶��
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
}