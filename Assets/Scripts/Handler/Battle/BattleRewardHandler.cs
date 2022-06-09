using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleRewardHandler : MonoBehaviour
{
    private List<GameObject> rewardObjs = new List<GameObject>();

    private InventoryHandler invenHandler;
    private InventoryInfoHandler invenInfo;
    private BattleHandler battleHandler;

    public BattleRewardUIHandler battleRewardUI;

    #region WaitForSeconds

    private readonly WaitForSeconds oneSecWait = new WaitForSeconds(1f);

    #endregion

    private void Start()
    {
        invenHandler = GameManager.Instance.inventoryHandler;
        battleHandler = GameManager.Instance.battleHandler;
        invenInfo = GameManager.Instance.invenInfoHandler;

        battleRewardUI.getBtn.onClick.AddListener(() =>
        {
            battleRewardUI.selectCG.interactable = false;
            battleRewardUI.buttonCG.interactable = false;


            GameManager.Instance.getPieceHandler.GetPiecePlayer(battleRewardUI.selectedSkillObj,
                () =>
                {
                    RemovePiece(null);

                    // ���� �� �˸�
                    battleRewardUI.SkipRewardEffect(() =>
                {
                    battleRewardUI.ResetRewardUI();
                    GameManager.Instance.EndEncounter();
                });
                },
                () =>
                {
                    RemovePiece(battleRewardUI.selectedSkillObj);

                    battleRewardUI.GetRewardEffect(() =>
                    {
                        battleRewardUI.ResetRewardUI();
                        int expValue = 0;
                        List<ExpLog> expLogs = new List<ExpLog>();
                        switch (GameManager.Instance.curEncounter)
                        {
                            case mapNode.BOSS:
                                expValue = 55;
                                expLogs.Add(new ExpLog("���� ���� ����ġ", expValue));
                                break;
                            case mapNode.EMONSTER:
                                expValue = 50;
                                expLogs.Add(new ExpLog("����Ʈ ���� ����ġ", expValue));
                                break;
                            case mapNode.MONSTER:
                                expValue = 30;
                                expLogs.Add(new ExpLog("�Ϲ� ���� ����ġ", expValue));
                                break;
                        }
                        GameManager.Instance.GetPlayer().AddExp(expValue, expLogs);
                    });
                });
        });

        battleRewardUI.skipBtn.onClick.AddListener(() =>
        {
            battleRewardUI.selectCG.interactable = false;
            battleRewardUI.buttonCG.interactable = false;

            RemovePiece(null);
            // ���� �� �˸�
            battleRewardUI.SkipRewardEffect(() =>
            {
                battleRewardUI.ResetRewardUI();
                int expValue = 0;
                List<ExpLog> expLogs = new List<ExpLog>();
                switch (GameManager.Instance.curEncounter)
                {
                    case mapNode.BOSS:
                        expValue = 55;
                        expLogs.Add(new ExpLog("���� ���� ����ġ", expValue));
                        break;
                    case mapNode.EMONSTER:
                        expValue = 50;
                        expLogs.Add(new ExpLog("����Ʈ ���� ����ġ", expValue));
                        break;
                    case mapNode.MONSTER:
                        expValue = 30;
                        expLogs.Add(new ExpLog("�Ϲ� ���� ����ġ", expValue));
                        break;
                }
                expValue += 70;
                expLogs.Add(new ExpLog("ī�� �ѱ�� ����ġ", 10));
                GameManager.Instance.GetPlayer().AddExp(expValue, expLogs);
            });
        });
    }

    public void Init(List<GameObject> rewardObjs)
    {
        this.rewardObjs = rewardObjs;
    }

    public void GiveReward()
    {
        StartCoroutine(ResetInventory(() =>
        {
            RewardRoutine();
        }));
    }

    public void ResetRullet(Action action)
    {
        StartCoroutine(ResetInventory(action));
    }

    public IEnumerator ResetInventory(Action action = null)
    {
        invenHandler.RemoveAllEnemyPiece();     // �� ��ų ���ְ�

        yield return null;

        PutRulletPieceInInventory();            // �귿 �� �κ��丮�� �ְ�

        yield return oneSecWait;
        yield return oneSecWait;

        action?.Invoke();

        yield break;
    }

    private void RewardRoutine()
    {
        List<SkillPiece> rewards = SetReward(rewardObjs, 3);

        battleRewardUI.ShowWinEffect(() =>
        {
            battleRewardUI.ShowReward(rewards);
        });
    }

    private void RemovePiece(SkillPiece selected)
    {
        for (int i = battleRewardUI.createdReward.Count - 1; i >= 0; i--)
        {
            SkillPiece skill = battleRewardUI.createdReward[i];

            if (skill != selected)
            {
                Destroy(skill.gameObject);
            }
        }

        battleRewardUI.createdReward.Clear();
    }

    private List<SkillPiece> SetReward(List<GameObject> rewardObjs, int rewardCnt = 8)
    {
        if (rewardObjs.Count < rewardCnt)
        {
            // ����
            return new List<SkillPiece>();
        }

        List<SkillPiece> randomRewards = new List<SkillPiece>();
        List<SkillPiece> result = new List<SkillPiece>();

        for (int i = 0; i < rewardObjs.Count; i++)
        {
            randomRewards.Add(rewardObjs[i].GetComponent<SkillPiece>());
        }

        for (int i = 0; i < rewardCnt; i++)
        {
            int randIdx = Random.Range(0, randomRewards.Count);

            SkillPiece reward = randomRewards[randIdx];
            result.Add(reward);

            randomRewards.RemoveAt(randIdx);
        }

        return result;
    }

    private void PutRulletPieceInInventory() //�귿�� �κ��丮�� �ٳ�������.
    {
        invenHandler.CycleSkills(); //���� > �κ�

        battleHandler.mainRullet.PutAllRulletPieceToInventory(); //�귿 > �κ�
    }
}
