using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RulletInventorySlot : InventorySlot
{
    // 현재 슬롯에 있는 조각
    [HideInInspector]
    public SkillPiece rulletPiece = null;

    // 룰렛 조각의 아이콘 스프라이트
    [HideInInspector]
    public Sprite iconSprite;

    // 낄 수 없다고 연출해주는거
    public Image cantEffectImg;

    // 슬롯에 있는 아이템을 장착할 때 불리는 액션
    public Action<int> equipSlot;

    public void Init()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (!hasItem)
                return;
            equipSlot(slotIdx);
        });
    }

    // 슬롯에 아이템을 추가할 때 불리는 함순데 좀 바꿀 필요 있음
    public void AddItem(SkillPiece _rulletPiece, Sprite _sprite, Sprite _iconSprite)
    {
        hasItem = true;
        rulletPiece = _rulletPiece;
        sprite = _sprite;
        iconSprite = _iconSprite;
        ShowItem();
    }

    // 현재 슬롯에 있는 아이템을 없엘 떄 불리는 함수
    public override void DeleteItem()
    {
        hasItem = false;
        rulletPiece = null;
        sprite = null;
        iconSprite = null;

        ShowItem();
    }

    // 아이템의 아이콘을 보여주는 함수
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
