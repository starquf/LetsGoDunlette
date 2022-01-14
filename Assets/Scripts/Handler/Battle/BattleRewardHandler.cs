using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRewardHandler : MonoBehaviour
{
    private List<GameObject> rewards = new List<GameObject>();

    private InventoryHandler inventoryHandler;
    private BattleHandler battleHandler;

    #region WaitForSeconds

    private readonly WaitForSeconds oneSecWait = new WaitForSeconds(1f);

    #endregion

    private void Start()
    {
        inventoryHandler = GameManager.Instance.inventoryHandler;
        battleHandler = GameManager.Instance.battleHandler;
    }

    public void Init(List<GameObject> rewards)
    {
        this.rewards = rewards;
    }

    public void GiveReward()
    {
        StartCoroutine(RewardRoutine());
    }

    private IEnumerator RewardRoutine()
    {
        PutRulletPieceInInventory(); //룰렛 다 인벤토리에 넣고
        yield return oneSecWait;
        yield return oneSecWait;
        inventoryHandler.RemoveAllEnemyPiece(); //적 룰렛 없애고

        CreateReward(); //적 보상을 주고

        // 끝

        // 전투 다시 시작 임시로 넣어둠
        battleHandler.StartBattle();
    }

    private void CreateReward()
    {
        Inventory owner = battleHandler.player.GetComponent<Inventory>();

        for (int i = 0; i < rewards.Count; i++)
        {
            inventoryHandler.CreateSkill(rewards[i], owner);
        }
    }

    private void PutRulletPieceInInventory() //룰렛을 인벤토리에 다넣으세요.
    {
        inventoryHandler.CycleSkills(); //무덤 > 인벤

        battleHandler.mainRullet.PutAllRulletPieceToInventory(); //룰렛 > 인벤
    }
}
