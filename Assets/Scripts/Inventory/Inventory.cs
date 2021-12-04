using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Inventory : MonoBehaviour
{
    // 인벤토리가 가지고 있는 슬롯의 정보
    public InventorySlot[] slots = new InventorySlot[8];

    // 슬롯들의 부모 트렌스폼
    public Transform slotHolder;

    protected virtual void Start()
    {
        slots = slotHolder.GetComponentsInChildren<InventorySlot>();
    }

    // 슬롯에 아이템을 추가할 때 불리는 함수
    public abstract void AddItem();

    // 슬롯에 아이템을 지울 때 불리는 함수
    public abstract void DeleteItem(int slotIdx);

    // 현재 인벤토리에 있는 모든 슬롯의 아이템 아이콘을 적용시켜주는 함수
    public virtual void ShowItem()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ShowItem();
        }
    }
}
