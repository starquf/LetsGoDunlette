using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Inventory : MonoBehaviour
{
    public InventorySlot[] slots = new InventorySlot[8];
    public Transform slotHolder;

    protected virtual void Start()
    {
        slots = slotHolder.GetComponentsInChildren<InventorySlot>();
    }

    public abstract void AddItem();

    public abstract void DeleteItem(int slotIdx);

    public virtual void ShowItem()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ShowItem();
        }
    }
}
