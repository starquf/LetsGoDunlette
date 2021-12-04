using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RulletInventory : Inventory
{
    //public new RulletInventorySlot[] slots = new RulletInventorySlot[8];
    public Transform rulletTrans;

    protected override void Start()
    {
        slots = slotHolder.GetComponentsInChildren<RulletInventorySlot>();

        Init();
    }

    private RulletInventorySlot[] ConvertRullet()
    {
        return (RulletInventorySlot[])slots;
    }

    private void Init()
    {
        RulletInventorySlot[] slots = ConvertRullet();

        Action<int> deleteSlot = null;
        deleteSlot = (slotIdx) =>
        {
            EquipRullet(slotIdx);
        };

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotIdx = i;
            slots[i].deleteEvent += deleteSlot;
            slots[i].Init();
        }
        RewardSetting();
    }

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

    public void AddItem(SkillPiece _rulletPiece, Sprite _sprite, Sprite _iconSprite)
    {
        RulletInventorySlot[] slots = ConvertRullet();

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].rulletPiece == null)
            {
                slots[i].AddItem(_rulletPiece, _sprite, _iconSprite);
                return;
            }
            if (i == slots.Length - 1)
            {
                DeleteItem(0);
                slots[i].AddItem(_rulletPiece, _sprite, _iconSprite);
            }
        }
    }
    public void EquipRullet(int slotIdx)
    {
        RulletInventorySlot[] slots = ConvertRullet();
        if (rulletTrans.GetComponent<SkillRullet>().IsRoll)
        {
            StartCoroutine(GameManager.Instance.inventoryHandler.CheckEquipRullet(rulletTrans.GetComponent<SkillRullet>(), slots[slotIdx], DeleteItem));
           
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

    public override void DeleteItem(int slotIdx)
    {
        RulletInventorySlot[] slots = ConvertRullet();

        slots[slotIdx].DeleteItem();
        for (int i = slotIdx + 1; i < slots.Length; i++)
        {
            slots[i - 1].rulletPiece = slots[i].rulletPiece;
            slots[i - 1].sprite = slots[i].sprite;
        }
        ShowItem();
    }

    public override void ShowItem()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ShowItem();
        }
    }
}
