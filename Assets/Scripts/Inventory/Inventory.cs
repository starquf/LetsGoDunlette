using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Inventory : MonoBehaviour
{
    // �κ��丮�� ������ �ִ� ������ ����
    public InventorySlot[] slots = new InventorySlot[8];

    // ���Ե��� �θ� Ʈ������
    public Transform slotHolder;

    protected virtual void Start()
    {
        slots = slotHolder.GetComponentsInChildren<InventorySlot>();
    }

    // ���Կ� �������� �߰��� �� �Ҹ��� �Լ�
    public abstract void AddItem();

    // ���Կ� �������� ���� �� �Ҹ��� �Լ�
    public abstract void DeleteItem(int slotIdx);

    // ���� �κ��丮�� �ִ� ��� ������ ������ �������� ��������ִ� �Լ�
    public virtual void ShowItem()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ShowItem();
        }
    }
}
