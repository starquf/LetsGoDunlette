using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class BattleRewardHandler : MonoBehaviour
{
    private List<GameObject> rewardObjs = new List<GameObject>();

    private InventoryHandler invenHandler;
    private BattleHandler battleHandler;

    public BattleRewardUIHandler battleRewardUI;

    #region WaitForSeconds

    private readonly WaitForSeconds oneSecWait = new WaitForSeconds(1f);

    #endregion

    private void Start()
    {
        invenHandler = GameManager.Instance.inventoryHandler;
        battleHandler = GameManager.Instance.battleHandler;

        battleRewardUI.getBtn.onClick.AddListener(() =>
        {
            battleRewardUI.selectCG.interactable = false;
            battleRewardUI.buttonCG.interactable = false;

            GetPiece(battleRewardUI.selectedSkillObj);
            RemovePiece(battleRewardUI.selectedSkillObj);

            battleRewardUI.GetRewardEffect(() =>
            {
                battleRewardUI.ResetRewardUI();
                GameManager.Instance.EndEncounter();
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
                GameManager.Instance.EndEncounter();
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
        PutRulletPieceInInventory();            //�귿 �� �κ��丮�� �ְ�
        invenHandler.RemoveAllEnemyPiece(); //�� ��ų ���ְ�

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

    private void GetPiece(SkillPiece skill, Action onEndCreate = null)
    {
        invenHandler.AddSkill(
            skill,
            battleHandler.player.GetComponent<Inventory>());
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
