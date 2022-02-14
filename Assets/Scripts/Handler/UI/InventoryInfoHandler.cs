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

    private float startPos;
    private float endPos;

    [Header("움직이는 것들")]
    public Transform bottomBG;

    private float bgStartPos;
    private float bgEndPos;

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

        startPos = cg.transform.localPosition.y;
        endPos = cg.transform.localPosition.y - rect.rect.height;

        bgStartPos = bottomBG.localPosition.y;
        bgEndPos = bottomBG.localPosition.y - 40f;

        contentRect = pieceHolderTrm.parent.GetComponent<RectTransform>();
        invenHandler = GameManager.Instance.inventoryHandler;

        invenBtn.onClick.AddListener(() =>
        {
            if (!isShow)
                ShowInventoryInfo();
        });
        //usedInvenBtn.onClick.AddListener(() => { ShowInfoPanel(true); });

        closeBtn.onClick.AddListener(() =>
        {
            desPanel.ShowPanel(false);
            ShowInfoPanel(false);
        });

        invenHandler.onUpdateInfo += ResetInventoryInfo;

        ShowInfoPanel(false);
        cg.alpha = 0f;
    }

    public void ShowInventoryInfo()
    {
        ShowInfoPanel(true);

        ResetInventoryInfo();
    }

    private void ResetPieceInfo()
    {
        for (int i = 0; i < pieceHolderTrm.childCount; i++)
        {
            pieceHolderTrm.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void ResetInventoryInfo()
    {
        if (!isShow) return;

        ResetPieceInfo();

        List<SkillPiece> skills = invenHandler.unusedSkills;

        for (int i = 0; i < skills.Count; i++)
        {
            Sprite bg = skills[i].cardBG;
            Sprite bookmark = invenHandler.effectSprDic[skills[i].patternType];
            Sprite bookmarkBG = invenHandler.bookmarkSprDic[skills[i].patternType];
            string name = skills[i].PieceName;
            string des = skills[i].PieceDes;

            PieceInfoUI pieceInfoUI = PoolManager.GetItem<PieceInfoUI>();
            pieceInfoUI.SetSkillIcon(skills[i].skillImg.sprite);

            pieceInfoUI.button.onClick.RemoveAllListeners();
            pieceInfoUI.button.onClick.AddListener(() =>
            {
                desPanel.ShowDescription(name, bg, des, bookmark, bookmarkBG);
            });

            pieceInfoUI.transform.SetAsFirstSibling();
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
    }

    private void ShowInfoPanel(bool enable)
    {
        SetCGEnable(enable);
        ShowCGEffect(enable);
    }

    private void SetCGEnable(bool enable)
    {
        cg.alpha = 1f;
        cg.blocksRaycasts = enable;
        cg.interactable = enable;

        isShow = enable;
    }

    private void ShowCGEffect(bool enable)
    {
        if (enable)
        {
            cg.transform.DOLocalMoveY(startPos, 0.35f)
                .SetEase(Ease.OutBack, 0.7f);

            bottomBG.transform.DOLocalMoveY(bgEndPos, 0.33f)
                .SetEase(Ease.OutQuad);
        }
        else
        {
            cg.transform.DOLocalMoveY(endPos, 0.22f)
                .SetEase(Ease.OutCubic);

            bottomBG.transform.DOLocalMoveY(bgStartPos, 0.33f)
                .SetEase(Ease.OutCubic);
        }
    }
}
