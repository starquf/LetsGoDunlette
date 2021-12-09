using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RulletInventory : Inventory
{
    public Transform rulletTrans;

    protected override void Start()
    {
        slots = slotHolder.GetComponentsInChildren<RulletInventorySlot>();

        Init();
    }

    // 인벤토리 슬롯으로 되있는 변수를 RulletInventorySlot으로 형변환
    private RulletInventorySlot[] ConvertRullet()
    {
        return (RulletInventorySlot[])slots;
    }

    private void Init()
    {
        // 현재 인벤토리에 있는 슬롯을 전부 받은 후
        RulletInventorySlot[] slots = ConvertRullet();

        Action<int> equipAction = null;
        equipAction = (slotIdx) =>
        {
            // 어떤 값을 주면 해당 값의 있는 슬롯에 있는 룰렛조각으로 교체하는 함수
            EquipRullet(slotIdx);
        };

        // 슬롯마다 액션을 걸어주는거고
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotIdx = i;
            slots[i].equipSlot += equipAction;
            slots[i].Init();
        }

        // 
        RewardSetting();
    }

    // 아이템 추가할 떄 불리는 함수
    public void RewardSetting()
    {
        Action<SkillPiece, Sprite, Sprite> addItemInventory = null;
        addItemInventory = (rulletPiece, sprite, _iconSprite) =>
        {
            AddItem(rulletPiece, sprite, _iconSprite);
        };

        GameManager.Instance.RewardEvent += addItemInventory;
    }

    public override void AddItem()
    {

    }

    // 슬롯에 아이템을 추가할 때 불리는 함순데 좀 바꿀 필요 있음
    public void AddItem(SkillPiece _rulletPiece, Sprite _sprite, Sprite _iconSprite)
    {
        // 현재 슬롯 가져온 다음
        RulletInventorySlot[] slots = ConvertRullet();

        for (int i = 0; i < slots.Length; i++)
        {
            // 공간이 남으면
            if (!slots[i].hasItem)
            {
                // 해당 슬롯에 아이템을 추가해준다
                slots[i].AddItem(_rulletPiece, _sprite, _iconSprite);
                return;
            }
            // 만약 꽉차면
            if (i == slots.Length - 1)
            {
                // 처음에 들어온걸 없에서 당겨서
                DeleteItem(0);
                // 넣어준다
                slots[i].AddItem(_rulletPiece, _sprite, _iconSprite);
            }
        }
    }

    // 해당 인덱스에 있는 슬롯의 룰렛조각으로 장착하는 함수
    public void EquipRullet(int slotIdx)
    {
        RulletInventorySlot[] slots = ConvertRullet();
        if (rulletTrans.GetComponent<SkillRullet>().IsRoll && !GameManager.Instance.battleHandler.IsTap && slots[slotIdx].hasItem)
        {
            GameManager.Instance.inventoryHandler.CheckEquitRulletStart(rulletTrans.GetComponent<SkillRullet>(), slots[slotIdx], DeleteItem);
        }
        else
        {
            Image effect = slots[slotIdx].cantEffectImg;
            Sequence cantEffect = DOTween.Sequence()
                .Append(effect.DOFade(1f, 0.1f))
                .Append(effect.DOFade(0f, 0.1f))
                .Append(effect.DOFade(1f, 0.1f))
                .Append(effect.DOFade(0f, 0.1f))
                .Append(effect.DOFade(1f, 0.1f))
                .Append(effect.DOFade(0f, 0.1f));
        }
    }

    // 해당 인덱스에 있는 아이템 삭제할때 불리는 함수
    public override void DeleteItem(int slotIdx)
    {
        RulletInventorySlot[] slots = ConvertRullet();

        slots[slotIdx].DeleteItem();

        //
        for (int i = slotIdx + 1; i < slots.Length; i++)
        {
            if(slots[i].hasItem)
            {
                slots[i].hasItem = false;
                slots[i - 1].rulletPiece = slots[i].rulletPiece;
                slots[i - 1].sprite = slots[i].sprite;
                slots[i - 1].iconSprite = slots[i].iconSprite;
                slots[i].rulletPiece = null;
                slots[i].sprite = null;
                slots[i].iconSprite = null;
                slots[i - 1].hasItem = true;
            }
        }

        ShowItem();
        //
    }

    // 현재 인벤토리에 있는 모든 슬롯의 아이템 아이콘을 적용시켜주는 함수
    public override void ShowItem()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ShowItem();
        }
    }
}
