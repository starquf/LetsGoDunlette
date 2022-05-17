using System.Collections;
using UnityEngine;

public class EnemyInventory : Inventory
{
    private float waitTime = 0.25f;

    protected virtual void Awake()
    {
        isPlayerInven = false;

        indicator = transform.GetChild(0).GetComponentInChildren<InventoryIndicator>();
    }

    public float CreateSkillsSmooth()
    {
        StartCoroutine(CreateSkillsWait());

        return (skillPrefabs.Count * waitTime) + 0.5f;
    }

    private IEnumerator CreateSkillsWait()
    {
        InventoryHandler inventoryHandler = GameManager.Instance.inventoryHandler;

        inventoryHandler.AddInventory(this);

        for (int i = 0; i < skillPrefabs.Count; i++)
        {
            SkillPiece skill = inventoryHandler.CreateSkill(skillPrefabs[i], this, transform.position);

            yield return new WaitForSeconds(waitTime);
        }
    }
}
