using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InventorySlot : MonoBehaviour
{
    // �����ܿ� �� ��������Ʈ
    [HideInInspector]
    public Sprite sprite;

    // ���� ������ ���°�ΰ�
    [HideInInspector]
    public int slotIdx;

    // �������� ������ �ִ°�?
    public bool hasItem;
    // ������ �̹���
    protected Image itemIcon;


    protected void Start()
    {
        itemIcon = transform.GetChild(0).GetComponent<Image>();
    }

    // ���Կ� �������� �߰��� �� �Ҹ��� �Լ�
    public virtual void AddItem()
    {
        hasItem = true;
    }

    // ���Կ� �������� ���� �� �Ҹ��� �Լ�
    public virtual void DeleteItem()
    {
        hasItem = false;
        ShowItem();
    }

    // �������� �������� �����ִ� �Լ�
    public virtual void ShowItem()
    {
        if (hasItem)
        {
            itemIcon.sprite = sprite;
            itemIcon.gameObject.SetActive(true);
        }
        else
        {
            itemIcon.gameObject.SetActive(false);
        }
    }
}
