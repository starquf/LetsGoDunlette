using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class EnemyReward : MonoBehaviour
{
    public List<GameObject> rewardObjs;

    public void GiveReward()
    {
        var inventory = GameManager.Instance.inventoryHandler;
        Inventory owner = GameManager.Instance.battleHandler.player.GetComponent<Inventory>();

        for (int i = 0; i < rewardObjs.Count; i++)
        {
            inventory.CreateSkill(rewardObjs[i], owner, transform.position);
        }
    }
}
