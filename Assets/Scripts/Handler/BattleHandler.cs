using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [Header("���� �귿 ���ߴ� �ڵ鷯")]
    public StopSliderHandler stopHandler;

    private InventoryHandler inventory;

    [Header("�� ����")]
    public Transform hpBar;
    public Transform hpShieldBar;
    public Text hpText;
    public Transform damageTrans;
    public Transform ccUITrm;

    //==================================================

    [Header("�귿��")]

    // ����, ���� or ���߿� �߰������� �𸣴� �귿
    public List<Rullet> rullets = new List<Rullet>();

    // ����� ���� �귿������
    public RulletPiece result;
    private int resultIdx;

    //==================================================

    public EnemyHealth[] enemys;

    [Header("�÷��̾�&�� Health")]
    public PlayerHealth player;
    public EnemyHealth enemy;

    private EnemyAttack enemyAtk;
    private EnemyReward enemyReward;

    //==================================================

    private bool isTap = false;
    public bool IsTap => isTap;
    
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
        MakeNewEnemy();
    }

    private IEnumerator RewardRoutine()
    {
        PutRulletPieceInInventory(); //�귿 �� �κ��丮�� �ְ�
        yield return oneSecWait;
        yield return oneSecWait;
        inventory.RemoveAllEnemyPiece(); //�� �귿 ���ְ�

        enemyReward.GiveReward(); //�� ������ �ְ�

        MakeNewEnemy(); //���ο� ���� �����

        StartCoroutine(InitRullet()); //�� ��ų �ְ�

        //��
    }

    private void MakeNewEnemy()
    {
        int index = UnityEngine.Random.Range(0,enemys.Length);
        enemy = Instantiate(enemys[index]);
        enemyAtk = enemy.GetComponent<EnemyAttack>();
        enemyReward = enemy.GetComponent<EnemyReward>();
    }

    private void PutRulletPieceInInventory() //�귿�� �κ��丮�� �ٳ�������.
    {
        inventory.CycleSkills(); //���� > �κ�
        SkillRullet rullet = rullets[0] as SkillRullet;
        rullet.PutAllRulletPieceToInventory(); //�귿 > �κ�
    }

    private void Start()
    {
        inventory = GameManager.Instance.inventoryHandler;

        // ���� & ��ž ��ư �߰�
        SetStopHandler();

        // ������ �����ϱ� �� �κ��丮�� �귿 ����
        StartCoroutine(InitRullet());
    }

    private void SetStopHandler()
    {
        if (stopHandler == null)
        {
            Debug.LogError("�ν����Ϳ��� BatteHandler�� ��ž �ڵ鷯�� �߰����ּ���!!");
            return;
        }

        stopHandler.Init(rullets[0], (result, pieceIdx) =>
        {
            stopHandler.rullet.HighlightResult();

            this.result = result;
            resultIdx = pieceIdx;
        });
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
        // ���� ��ų�� �߰����ش�
        yield return new WaitForSeconds(enemyAtk.AddAllSkills() + 0.5f);

        // �κ��丮���� ������ 6���� ��ų�� �̾� �귿�� �����Ѵ�. ��, �ּ��� ���� ��ų 1���� �� ��ų 2���� ����ȴ�.

        // �÷��̾� Ȯ�� 2��
        for (int i = 0; i < 2; i++)
        {
            SetRandomPlayerOrEnemySkill(true);
            yield return new WaitForSeconds(0.15f);
        }

        // �� Ȯ�� �ϳ�
        SetRandomPlayerOrEnemySkill(false);
        yield return new WaitForSeconds(0.15f);

        // ������ ���� 3��
        for (int i = 0; i < 3; i++)
        {
            SetRandomSkill();

            yield return new WaitForSeconds(0.15f);
        }

        yield return oneSecWait;

        // �� ����
        InitTurn();
    }

    // ���� ������ �Ѿ�� ��
    private void InitTurn()
    {
        // ��ư �ʱ�ȭ
        stopHandler.SetInteract(false);

        result = null;

        turnCnt++;

        // ���� �Ͽ� �ɷ��ִ� ���� cc��� �÷��̾��� cc�⸦ �ϳ� �ٿ��ش�.
        DecreaseCC();

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
        //��ư Ȱ��ȭ
        stopHandler.SetInteract(true);

        // ���� ���� ������
        yield return new WaitUntil(CheckRullet);

        // ��� �����ְ�
        yield return oneSecWait;

        // ����� �����س��� �� ĭ�� null�� ������ش�
        ExeptRulletResult(resultIdx);

        // ��� ����
        CastResult();
    }

    // ������ ���� ������ ����Ǵ� �ڷ�ƾ
    private IEnumerator EndTurn()
    {
        nextAttack?.Invoke(result);
        // �귿 ������ �κ��丮�� �˾Ƽ� ����
        yield return pFiveSecWait;

        // ������ ����� �κ��丮�� �ִ´�
        SetUseRulletPiece(result as SkillPiece);

        // �����̶�� ������ ����������
        CheckStun();

        // ��� ��ٸ���
        yield return oneSecWait;

        // ���� �׾��°�?
        if (enemy.IsDie)
        {
            StartCoroutine(RewardRoutine());
            yield break;
        }

        yield return pFiveSecWait;

        // �귿 ���� ���� (����ȯ)
        DrawRulletPieces();

        yield return pFiveSecWait;

        // ����������
        InitTurn();
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
        }
        else
        {

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
        rullets[0].GetComponent<SkillRullet>().PutRulletPieceToGraveYard(pieceIdx);
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

    private void DecreaseCC()
    {
        player.cc.DecreaseAllTurn();
        enemy.cc.DecreaseAllTurn();
    }

    private void CheckStun()
    {
        // �����Ǿ��ִٸ�
        if (player.cc.ccDic[CCType.Stun] > 0)
        {
            Stun(true);
        }

        if (enemy.cc.ccDic[CCType.Stun] > 0)
        {
            Stun(false);
        }
    }

    private void Stun(bool isPlayer)
    {
        List<RulletPiece> pieces = rullets[0].GetPieces();

        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i] == null)
            {
                continue;
            }

            // �÷��̾��� �����̶��
            if ((pieces[i] as SkillPiece).isPlayerSkill.Equals(isPlayer))
            {
                (rullets[0] as SkillRullet).PutRulletPieceToGraveYard(i);
            }
        }
    }


    //��ư ���µ�

    // stop ��Ȱ��
    // stop Ȱ�� // �⺻
    // ���� Ȱ��
    // ���� ��Ȱ��
}
