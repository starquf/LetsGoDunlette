using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHandler : MonoBehaviour
{
    //==================================================

    [Header("���� �귿 ���ߴ� �ڵ鷯")]
    public StopSliderHandler stopHandler;

    [Header("ĳ��Ʈ �ϴ� ��ų ����ִ� �ڵ鷯")]
    public PieceCastUIHandler castUIHandler;

    private BattleInfoHandler battleInfoHandler;
    private CCHandler ccHandler;
    private BattleRewardHandler battleRewardHandler;
    private BattleTargetSelectHandler battleTargetSelector;

    private InventoryHandler inventory;

    //==================================================

    private BattleInfo battleInfo;

    [Header("���� �����ϴ� ��ġ")]
    public Transform createTrans;

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
    public SkillPiece result;
    private int resultIdx;

    //==================================================

    [Header("�÷��̾�&�� Health")]
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

        // �÷��̾ ������ �ִ� �⺻ ��ų ���� �ϴ� �׽�Ʈ�� ������
        player.GetComponent<Inventory>().CreateSkills();

        //StartBattle();
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
            enemys.Add(Instantiate(enemyInfos[i]));
        }

        // ������ ����� �ٲ���ߵȴ� 
        SortBoss();
        SetEnemyPosition();
    }

    private void SortBoss()
    {
        int idx = enemys.Count / 2;

        for (int i = 0; i < enemys.Count; i++)
        {
            // ������
            if (enemys[i].isBoss && i != idx)
            {
                EnemyHealth temp = enemys[idx];

                enemys[idx] = enemys[i];
                enemys[i] = temp;
            }
        }
    }

    private void SetEnemyPosition()
    {
        if (enemys.Count <= 0) return;

        Vector2 screenX = new Vector2(Camera.main.ViewportToWorldPoint(Vector3.zero).x, Camera.main.ViewportToWorldPoint(Vector3.one).x);
        float posX = (Mathf.Abs(screenX.x) + screenX.y) / (float)(enemys.Count + 1);

        // -5  5   10   5   

        float totalX = 0f;

        for (int i = 0; i < enemys.Count; i++)
        {
            totalX += posX;

            enemys[i].transform.position = createTrans.position;
            enemys[i].transform.DOMoveX(totalX + screenX.x, 0.3f);
        }
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

            this.result = result as SkillPiece;
            resultIdx = pieceIdx;
        });
    }

    #endregion

    #region Init Rullet

    private IEnumerator InitRullet()
    {
        yield return null;

        // ���� ��ų�� �߰����ش�
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
        // ��ó üũ
        ccHandler.CheckCC(CCType.Wound);

        // ��� ��ٸ���
        yield return oneSecWait;

        // ���� ���� �׾��°�?
        if (enemys[0].IsDie)
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
        enemys.Clear();
        battleRewardHandler.GiveReward();
    }

    #endregion

    #region Rullet Func

    // ��� �����ֱ�
    private void CastResult()
    {
        if (result != null)
        {
            castUIHandler.ShowCasting(result.skillImg.sprite);

            // �÷��̾� ��ų�̶��
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
                result.Cast(player, () =>
                {
                    castUIHandler.ShowPanel(false);
                    StartCoroutine(EndTurn());
                });
            }
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
