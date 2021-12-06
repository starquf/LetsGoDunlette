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

    // �κ��丮 �������� ���ִ� ������ RulletInventorySlot���� ����ȯ
    private RulletInventorySlot[] ConvertRullet()
    {
        return (RulletInventorySlot[])slots;
    }

    private void Init()
    {
        // ���� �κ��丮�� �ִ� ������ ���� ���� ��
        RulletInventorySlot[] slots = ConvertRullet();

        Action<int> equipAction = null;
        equipAction = (slotIdx) =>
        {
            // � ���� �ָ� �ش� ���� �ִ� ���Կ� �ִ� �귿�������� ��ü�ϴ� �Լ�
            EquipRullet(slotIdx);
        };

        // ���Ը��� �׼��� �ɾ��ִ°Ű�
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotIdx = i;
            slots[i].equipSlot += equipAction;
            slots[i].Init();
        }

        // 
        RewardSetting();
    }

    // ������ �߰��� �� �Ҹ��� �Լ�
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

    // ���Կ� �������� �߰��� �� �Ҹ��� �Լ��� �� �ٲ� �ʿ� ����
    public void AddItem(SkillPiece _rulletPiece, Sprite _sprite, Sprite _iconSprite)
    {
        // ���� ���� ������ ����
        RulletInventorySlot[] slots = ConvertRullet();

        for (int i = 0; i < slots.Length; i++)
        {
            // ������ ������
            if (slots[i].rulletPiece == null)
            {
                // �ش� ���Կ� �������� �߰����ش�
                slots[i].AddItem(_rulletPiece, _sprite, _iconSprite);
                return;
            }
            // ���� ������
            if (i == slots.Length - 1)
            {
                // ó���� ���°� ������ ��ܼ�
                DeleteItem(0);
                // �־��ش�
                slots[i].AddItem(_rulletPiece, _sprite, _iconSprite);
            }
        }
    }

    // �ش� �ε����� �ִ� ������ �귿�������� �����ϴ� �Լ�
    public void EquipRullet(int slotIdx)
    {
        RulletInventorySlot[] slots = ConvertRullet();
        if (rulletTrans.GetComponent<SkillRullet>().IsRoll && !GameManager.Instance.battleHandler.IsTap)
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

    // �ش� �ε����� �ִ� ������ �����Ҷ� �Ҹ��� �Լ�
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

    // ���� �κ��丮�� �ִ� ��� ������ ������ �������� ��������ִ� �Լ�
    public override void ShowItem()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ShowItem();
        }
    }
}
