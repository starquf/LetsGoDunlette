using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("������ �ִ� ��ų�� ������")]
    public List<GameObject> skillPrefabs = new List<GameObject>();

    //[HideInInspector]
    public List<SkillPiece> skills = new List<SkillPiece>();

    public InventoryIndicator indicator;

    [HideInInspector]
    public bool isPlayerInven = false;

    public virtual void CreateSkills()
    {
        InventoryHandler inventoryHandler = GameManager.Instance.inventoryHandler;

        for (int i = 0; i < skillPrefabs.Count; i++)
        {
            inventoryHandler.CreateSkill(skillPrefabs[i], this);
        }

        inventoryHandler.AddInventory(this);
    }


}
