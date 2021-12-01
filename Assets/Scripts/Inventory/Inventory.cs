using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public InventorySlot[] slots = new InventorySlot[8];
    public Transform slotHolder;
    public Transform rulletTrans;
    public GameObject skillPiecePrefab;

    void Start()
    {
        slots = slotHolder.GetComponentsInChildren<InventorySlot>();

        Init();
    }

    private void Init()
    {
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

    public void AddItem(SkillPiece _rulletPiece, Sprite _sprite, Sprite _iconSprite)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].rulletPiece == null)
            {
                slots[i].AddItem(_rulletPiece, _sprite, _iconSprite);
                return;
            }
            if(i == slots.Length-1)
            {
                DeleteItem(0);
                slots[i].AddItem(_rulletPiece, _sprite, _iconSprite);
            }
        }
    }
    public void EquipRullet(int slotIdx)
    {
        GameObject item = Instantiate(skillPiecePrefab, transform.position, Quaternion.identity, rulletTrans);

        item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y, 0f);
        item.transform.localScale = new Vector3(1f, 1f, 1f);
        item.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        item.GetComponent<Image>().sprite = slots[slotIdx].sprite;

        SkillPiece skillPieceItem = item.GetComponent<SkillPiece>();
        skillPieceItem.ChangeSize(slots[slotIdx].rulletPiece.Size);
        skillPieceItem.skillImg.sprite = slots[slotIdx].iconSprite;
        skillPieceItem.ChangePieceName(slots[slotIdx].rulletPiece.PieceName);

        GameManager.Instance.inventoryHandler.EquipRullet();

        DeleteItem(slotIdx);

        Sequence itemSeq = DOTween.Sequence()
            .Append(item.GetComponent<Image>().DOFade(1f, 1f).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
                rulletTrans.GetComponent<Rullet>().AddPiece(item.GetComponent<RulletPiece>());
            })
            .Append(item.transform.DOLocalMove(Vector3.zero, 1f))
            .AppendCallback(() =>
            {
                GameManager.Instance.inventoryHandler.EndEquipRullet();
            });
    }

    public void DeleteItem(int slotIdx)
    {
        slots[slotIdx].DeleteItem();
        for (int i = slotIdx+1; i < slots.Length; i++)
        {
            slots[i - 1].rulletPiece = slots[i].rulletPiece;
            slots[i - 1].sprite = slots[i].sprite;
        }
        ShowItem();
    }

    public void ShowItem()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ShowItem();
        }
    }
}
