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

    //==================================================

    [Header("���� �귿 ���ߴ� �ڵ鷯")]
    public StopSliderHandler stopHandler;

    private BattleInfoHandler battleInfoHandler;
    private CCHandler ccHandler;
    private BattleRewardHandler battleRewardHandler;

    private InventoryHandler inventory;

    //==================================================

    private BattleInfo battleInfo;

    [Header("�� ����")]
    public Transform hpBar;
    public Transform hpShieldBar;
    public Text hpText;
    public Transform damageTrans;
    public Transform ccUITrm;

    //==================================================

    [Header("�귿��")]

    // ���� �귿
    public SkillRullet mainRullet;

    // ����� ���� �귿����
    [HideInInspector]
    public RulletPiece result;
    private int resultIdx;

    //==================================================

    [Header("�÷��̾�&�� Health")]
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

        // �÷��̾ ������ �ִ� �⺻ ��ų ���� �ϴ� �׽�Ʈ�� ������
        player.GetComponent<Inventory>().CreateSkills();
    }

    #region StartBattle

    // ������ �����ϴ� �Լ�
    public void StartBattle()
    {
        onNextAttack = null;
        nextAttack = null;

        // ���� ���� ���� ��������
        battleInfo = battleInfoHandler.GetRandomBattleInfo();

        // �� ���� �ϴ� �׽�Ʈ�� �ϳ��� ����
        CreateEnemy(battleInfo.enemyInfos);

        // �ڵ鷯�� �ʱ�ȭ
        InitHandler();

        // ��ž ��ư�� ��� �߰�
        SetStopHandler();

        // ������ �����ϱ� �� �κ��丮�� �귿 ����
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
            Debug.LogError("�ν����Ϳ��� BatteHandler�� ��ž �ڵ鷯�� �߰����ּ���!!");
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

        // ���� ��ų�� �߰����ش�
        yield return new WaitForSeconds(enemyInventory.CreateSkillsSmooth() + 0.5f);

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

    // ���� ������ �Ѿ�� ��
    private void InitTurn()
    {
        // ��ư �ʱ�ȭ
        stopHandler.SetInteract(false);

        result = null;

        turnCnt++;

        // ���� �Ͽ� �ɷ��ִ� ���� cc��� �÷��̾��� cc�⸦ �ϳ� �ٿ��ش�.
        ccHandler.DecreaseCC();

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
        stopHandler.SetInteract(true);

        // ���� ���� ������ ��ٸ���
        yield return new WaitUntil(CheckRullet);

        // ��� �����ְ�
        yield return oneSecWait;

        // ����� �����س��� �� ĭ�� ��ĭ���� ������ش�
        SetRulletEmpty(resultIdx);

        // ��� ����
        CastResult();
    }

    // ������ ���� ������ ����Ǵ� �ڷ�ƾ
    private IEnumerator EndTurn()
    {
        // ���� ���� üũ�ϴ� ��ų���� �ߵ��Ǵ� Ÿ�̹�
        nextAttack?.Invoke(result);

        yield return pFiveSecWait;

        // ������ ����� �κ��丮�� �ִ´�
        SetPieceToGraveyard(result as SkillPiece);

        // ���� üũ
        ccHandler.CheckCC(CCType.Stun);

        // ��� ��ٸ���
        yield return oneSecWait;

        // ���� �׾��°�?
        if (enemy.IsDie)
        {
            BattleEnd();
            yield break;
        }

        yield return pFiveSecWait;

        // �귿 ���� ���� (����ȯ)
        DrawRulletPieces();

        yield return pFiveSecWait;

        // ����������
        InitTurn();
    }

    // ������ ���� ��
    private void BattleEnd()
    {
        battleRewardHandler.GiveReward();
    }

    #endregion

    #region Rullet Func

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
            // ����ִ°��̶��
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
