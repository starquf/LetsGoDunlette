using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryInfoHandler : MonoBehaviour
{
    private CanvasGroup cg;
    private RectTransform rect;
    private RectTransform contentRect;

    private Vector3 startPos;

    [Header("버튼")]
    public Button invenBtn;
    public Button usedInvenBtn;
    public Button closeBtn;

    [Header("조각 홀더 관련")]
    public Transform pieceHolderTrm;
    public GameObject pieceInfoObj;

    public PieceDesUIHandler desPanel;

    private InventoryHandler invenHandler;

    private bool isShow = false;

    private void Awake()
    {
        PoolManager.CreatePool<PieceInfoUI>(pieceInfoObj, pieceHolderTrm, 5);
    }

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
        startPos = cg.transform.localPosition;

        contentRect = pieceHolderTrm.parent.GetComponent<RectTransform>();
        invenHandler = GameManager.Instance.inventoryHandler;

        invenBtn.onClick.AddListener(() => 
        {
            if(!isShow)
                ShowInventoryInfo(); 
        });
        //usedInvenBtn.onClick.AddListener(() => { ShowInfoPanel(true); });

        closeBtn.onClick.AddListener(() => 
        {
            desPanel.ShowPanel(false);
            ShowInfoPanel(false);
        });

        ShowInfoPanel(false);
    }

    public void ShowInventoryInfo()
    {
        ResetPieceInfo();

        List<SkillPiece> skills = invenHandler.unusedSkills;

        for (int i = 0; i < skills.Count; i++)
        {
            Sprite icon = skills[i].skillImg.sprite;
            string name = skills[i].PieceName;
            string des = skills[i].PieceDes;

            PieceInfoUI pieceInfoUI = PoolManager.GetItem<PieceInfoUI>();
            pieceInfoUI.SetSkillIcon(icon);

            pieceInfoUI.button.onClick.RemoveAllListeners();
            pieceInfoUI.button.onClick.AddListener(() =>
            {
                desPanel.ShowDescription(name, icon, des);
            });

            pieceInfoUI.transform.SetAsFirstSibling();
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);

        ShowInfoPanel(true);
    }

    private void ResetPieceInfo()
    {
        for (int i = 0; i < pieceHolderTrm.childCount; i++)
        {
            pieceHolderTrm.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void ShowInfoPanel(bool enable)
    {
        cg.alpha = enable ? 1f : 0f;
        cg.blocksRaycasts = enable;
        cg.interactable = enable;

        cg.transform.localPosition = startPos;

        isShow = enable;

        if (enable)
        {
            cg.transform.DOLocalMoveY(-rect.rect.height, 0.35f)
                .From(true)
                .SetEase(Ease.OutBack, 0.7f);
        }
    }
}
