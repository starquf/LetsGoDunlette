using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class EnemyReward : MonoBehaviour
{
    public Transform rulletTrans;

    private bool isReward = false;
    public bool IsReward => isReward;

    public List<GameObject> rewardObjs;

    public void GiveReward()
    {
        var inventory = GameManager.Instance.inventoryHandler;
        for (int i = 0; i < rewardObjs.Count; i++)
        {
            inventory.CreateSkill(rewardObjs[i],transform.position);
        }
    }
}
