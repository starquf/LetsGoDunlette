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
        PutRulletPieceInInventory(); //�귿 �� �κ��丮�� �ְ�
        yield return oneSecWait;
        yield return oneSecWait;
        inventoryHandler.RemoveAllEnemyPiece(); //�� �귿 ���ְ�

        CreateReward(); //�� ������ �ְ�

        // ��

        // ���� �ٽ� ���� �ӽ÷� �־��
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

    private void PutRulletPieceInInventory() //�귿�� �κ��丮�� �ٳ�������.
    {
        inventoryHandler.CycleSkills(); //���� > �κ�

        battleHandler.mainRullet.PutAllRulletPieceToInventory(); //�귿 > �κ�
    }
}
