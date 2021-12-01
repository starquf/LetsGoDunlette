using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RulletInventorySlot : InventorySlot
{
    [HideInInspector]
    public SkillPiece rulletPiece = null;
    [HideInInspector]
    public Sprite iconSprite;
    [HideInInspector]
    public int slotIdx;

    public Action<int> deleteEvent;

    public void Init()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (rulletPiece == null)
                return;
            deleteEvent(slotIdx);
        });
    }

    public void AddItem(SkillPiece _rulletPiece, Sprite _sprite, Sprite _iconSprite)
    {
        hasItem = true;
        rulletPiece = _rulletPiece;
        sprite = _sprite;
        iconSprite = _iconSprite;
        ShowItem();
    }

    public override void DeleteItem()
    {
        hasItem = false;
        rulletPiece = null;
        sprite = null;
        iconSprite = null;
        ShowItem();
    }

    public override void ShowItem()
    {
        if(hasItem)
        {
            itemIcon.sprite = sprite;
            itemIcon.type = Image.Type.Filled;
            itemIcon.fillMethod = Image.FillMethod.Radial360;
            itemIcon.fillOrigin = (int)Image.Origin360.Top;
            itemIcon.fillAmount = (float)rulletPiece.Size/36f;

            itemIcon.gameObject.SetActive(true);
        }
        else
        {
            itemIcon.gameObject.SetActive(false);
        }
    }
}
