using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InventorySlot : MonoBehaviour
{
    [HideInInspector]
    public Sprite sprite;

    protected bool hasItem;
    protected Image itemIcon;


    protected void Start()
    {
        itemIcon = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void AddItem()
    {
        hasItem = true;
    }

    public virtual void DeleteItem()
    {
        hasItem = false;
        ShowItem();
    }

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
