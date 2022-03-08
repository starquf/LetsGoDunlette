using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PieceDesUIHandler : MonoBehaviour
{
    private InventoryHandler invenHandler;

    public Text nameText;
    public Image bgImg;
    public Text desText;
    public Image bookmarkImg;
    public Image bookmarkBGImg;

    [Space(10f)]
    public Button closeBtn;
    public Button confirmBtn;

    private CanvasGroup cg;
    private RectTransform rect;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();

        invenHandler = GameManager.Instance.inventoryHandler;

        closeBtn.onClick.AddListener(() => ShowPanel(false));

        ShowPanel(false);
    }

    public void ShowDescription(SkillPiece skillPiece)
    {
        Sprite bg = skillPiece.cardBG;
        Sprite bookmark = invenHandler.effectSprDic[skillPiece.currentType];
        Sprite bookmarkBG = invenHandler.bookmarkSprDic[skillPiece.currentType];
        string name = skillPiece.PieceName;
        string des = skillPiece.PieceDes;

        nameText.text = name;
        bgImg.sprite = bg;
        desText.text = des;
        bookmarkImg.sprite = bookmark;
        bookmarkBGImg.sprite = bookmarkBG;

        confirmBtn.gameObject.SetActive(false);

        ShowPanel(true);
    }

    public void ShowConfirmBtn(Action onConfirm)
    {
        confirmBtn.gameObject.SetActive(true);
        confirmBtn.onClick.RemoveAllListeners();
        confirmBtn.onClick.AddListener(() => onConfirm?.Invoke());
    }

    public void ShowPanel(bool enable)
    {
        cg.alpha = enable ? 1f : 0f;
        cg.blocksRaycasts = enable;
        cg.interactable = enable;
    }
}
