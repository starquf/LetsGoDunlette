using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    private bool isEquip = false;
    public bool IsEquip => isEquip;
    public Button OpenBtn;

    public CanvasGroup InventoryCvsGroup;
    private bool isInventoryOpened = false;


    private void Start()
    {
        GameManager.Instance.inventoryHandler = this;
        OpenBtn.onClick.AddListener(() =>
        {
            OpenInventory();
        });
    }

    public void EquipRullet()
    {
        isEquip = true;
        InventoryCvsGroup.alpha = 0f;
    }
    public void EndEquipRullet()
    {
        isEquip = false;
        InventoryCvsGroup.alpha = 1f;
    }

    public void OpenInventory()
    {
        if (isEquip)
            return;
        isInventoryOpened = !isInventoryOpened;
        InventoryCvsGroup.alpha = isInventoryOpened ? 1f : 0f;
        InventoryCvsGroup.interactable = isInventoryOpened;
        InventoryCvsGroup.blocksRaycasts = isInventoryOpened;
    }
}
