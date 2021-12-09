using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InventorySlot : MonoBehaviour
{
    // 아이콘에 들어갈 스프라이트
    [HideInInspector]
    public Sprite sprite;

    // 현재 슬롯이 몇번째인가
    [HideInInspector]
    public int slotIdx;

    // 아이템을 가지고 있는가?
    public bool hasItem;
    // 아이콘 이미지
    protected Image itemIcon;


    protected void Start()
    {
        itemIcon = transform.GetChild(0).GetComponent<Image>();
    }

    // 슬롯에 아이템을 추가할 때 불리는 함수
    public virtual void AddItem()
    {
        hasItem = true;
    }

    // 슬롯에 아이템을 지울 때 불리는 함수
    public virtual void DeleteItem()
    {
        hasItem = false;
        ShowItem();
    }

    // 아이템의 아이콘을 보여주는 함수
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
