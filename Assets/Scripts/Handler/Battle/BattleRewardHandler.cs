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

            GetPiece(battleRewardUI.selectedSkillObj.gameObject);

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

            // 전투 끝 알림
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
        PutRulletPieceInInventory();            //룰렛 다 인벤토리에 넣고
        invenHandler.RemoveAllEnemyPiece(); //적 스킬 없애고

        yield return oneSecWait;
        yield return oneSecWait;

        action?.Invoke();

        yield break;
    }

    private void RewardRoutine()
    {
        List<SkillPiece> rewards = SetReward(rewardObjs, 6);

        battleRewardUI.ShowWinEffect(() =>
        {
            battleRewardUI.ShowReward(rewards);
        });
    }

    private void GetPiece(GameObject skillObj, Action onEndCreate = null)
    {
        /*
        invenHandler.CreateSkill(
            skillObj, 
            battleHandler.player.GetComponent<Inventory>(), 
            battleRewardUI.pieceDesCG.transform.position,
            () => { 
                onEndCreate?.Invoke();
            });
        */
        invenHandler.CreateSkill(
            skillObj,
            battleHandler.player.GetComponent<Inventory>());
    }

    private List<SkillPiece> SetReward(List<GameObject> rewardObjs, int rewardCnt = 8)
    {
        if (rewardObjs.Count < rewardCnt)
        {
            // 오류
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

    private void PutRulletPieceInInventory() //룰렛을 인벤토리에 다넣으세요.
    {
        invenHandler.CycleSkills(); //무덤 > 인벤

        battleHandler.mainRullet.PutAllRulletPieceToInventory(); //룰렛 > 인벤
    }
}
