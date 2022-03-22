using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInventory : Inventory
{
    private float waitTime = 0.25f;

    public float CreateSkillsSmooth()
    {
        StartCoroutine(CreateSkillsWait());

        return skillPrefabs.Count * waitTime + 0.5f;
    }

    private IEnumerator CreateSkillsWait()
    {
        InventoryHandler inventoryHandler = GameManager.Instance.inventoryHandler;

        for (int i = 0; i < skillPrefabs.Count; i++)
        {
            SkillPiece skill = inventoryHandler.CreateSkill(skillPrefabs[i], this, transform.position);

            yield return new WaitForSeconds(waitTime);
        }
    }
}
