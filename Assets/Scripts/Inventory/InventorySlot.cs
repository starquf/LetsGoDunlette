using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public SkillPiece rulletPiece = null;
    public Sprite sprite;
    public Sprite iconSprite;
    [HideInInspector]
    public int slotIdx;

    private Image itemIcon;
    private Text debugTxt; // ���ﲨ

    public Action<int> deleteEvent;

    private void Start()
    {
        itemIcon = transform.GetChild(0).GetComponent<Image>();
        debugTxt = transform.GetChild(1).GetComponent<Text>();
    }
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
        rulletPiece = _rulletPiece;
        sprite = _sprite;
        iconSprite = _iconSprite;
        ShowItem();
    }

    public void DeleteItem()
    {
        rulletPiece = null;
        sprite = null;
        iconSprite = null;
        ShowItem();
    }

    public void ShowItem()
    {
        if(rulletPiece != null)
        {
            //��������Ʈ�� �޾ƿ��°� �ؾߵǰ� �ϴ� �ؽ�Ʈ��
            debugTxt.text = rulletPiece.PieceName;
            debugTxt.gameObject.SetActive(true);

            itemIcon.sprite = sprite;
            itemIcon.type = Image.Type.Filled;
            itemIcon.fillMethod = Image.FillMethod.Radial360;
            itemIcon.fillOrigin = (int)Image.Origin360.Top;
            itemIcon.fillAmount = (float)rulletPiece.Size/36f;

            itemIcon.gameObject.SetActive(true);
        }
        else
        {
            debugTxt.gameObject.SetActive(false);//���ﲨ
            itemIcon.gameObject.SetActive(false);
        }
    }
}
