using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct RewardChance
{
    public int gradeOne;
    public int gradeTwo;
    public int gradeThree;
}

public class BattleRewardHandler : MonoBehaviour
{
    private InventoryHandler invenHandler;
    private InventoryInfoHandler invenInfo;
    private BattleHandler battleHandler;

    public BattleRewardUIHandler battleRewardUI;

    public List<RewardChance> rewardChances;

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
                        GiveExp(false);
                        GoldReward();
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
                GiveExp(true);
                GoldReward();
            });
        });
    }

    private void GoldReward()
    {
        int goldValue = 0;
        switch (GameManager.Instance.curEncounter)
        {
            case mapNode.BOSS:
                goldValue = Random.Range(72, 80);
                break;
            case mapNode.EMONSTER:
                goldValue = Random.Range(26, 36);
                break;
            case mapNode.MONSTER:
                goldValue = Random.Range(10, 21);
                break;
        }
        GameManager.Instance.AddGold(goldValue);
    }

    private void GiveExp(bool isSkip)
    {
        if (GameManager.Instance.GetPlayer().IsMaxLevel())
        {
            GameManager.Instance.EndEncounter();
        }
        else
        {
            int expValue = 0;
            List<ExpLog> expLogs = new List<ExpLog>();
            switch (GameManager.Instance.curEncounter)
            {
                case mapNode.BOSS:
                    expValue = 45;
                    expLogs.Add(new ExpLog("���� ���� ����ġ", expValue));
                    break;
                case mapNode.EMONSTER:
                    expValue = 40;
                    expLogs.Add(new ExpLog("����Ʈ ���� ����ġ", expValue));
                    break;
                case mapNode.MONSTER:
                    expValue = 25;
                    expLogs.Add(new ExpLog("�Ϲ� ���� ����ġ", expValue));
                    break;
            }
            if (isSkip)
            {
                expValue += 20;
                expLogs.Add(new ExpLog("ī�� �ѱ�� ����ġ", 20));
            }
            GameManager.Instance.GetPlayer().AddExp(expValue, expLogs);
        }
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

    private List<SkillPiece> GetRewardList()
    {
        List<SkillPiece> rewards = null;
        int playerLevel = (int)Mathf.Clamp(GameManager.Instance.GetPlayer().PlayerLevel - 1, 0, Mathf.Infinity);

        List<SkillPiece> skills;

        switch (GameManager.Instance.curEncounter)
        {
            case mapNode.MONSTER:
                {
                    rewards = GameManager.Instance.skillContainer.GetSkillsByChance(rewardChances[playerLevel], 3);
                    //rewards = SetReward(rewardObjs, 3);
                }
                break;
            case mapNode.EMONSTER: //����Ʈ ���� ������ 2�� ���� 1�� Ȯ��
                {
                    skills = GameManager.Instance.skillContainer.GetSkillsByGrade(GradeInfo.Epic);
                    rewards = SetReward(skills, 1);
                    foreach (var item in GameManager.Instance.skillContainer.GetSkillsByChance(rewardChances[playerLevel], 2))
                    {
                        rewards.Add(item);
                    }
                }
                break;
            case mapNode.BOSS: //���� ���� ������ 3�� ���� 3�� Ȯ��
                {
                    skills = GameManager.Instance.skillContainer.GetSkillsByGrade(GradeInfo.Legend);
                    rewards = SetReward(skills, 3);
                }
                break;
        }

        return rewards;
    }

    private void RewardRoutine()
    {
        List<SkillPiece> rewards = GetRewardList();

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

    private List<SkillPiece> SetReward(List<SkillPiece> rewardObjs, int rewardCnt = 8)
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
