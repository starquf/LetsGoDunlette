using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class InventoryInfoHandler : MonoBehaviour
{
    private CanvasGroup cg;
    private RectTransform rect;
    private RectTransform contentRect;

    public CanvasGroup pauseCg;

    private float startPos;
    private float endPos;

    public Canvas upCvs;
    private string ignoreEft = "IgnoreEffect";
    private string upUI = "UpUI";

    [Header("움직이는 것들")]
    public BottomUIHandler bottomBG;

    [Header("버튼")]
    public Button invenBtn;
    public Button usedInvenBtn;
    public Button closeBtn;

    [Header("조각 홀더 관련")]
    public Transform pieceHolderTrm;
    public GameObject pieceInfoObj;

    [Header("하이라이트 관련")]
    public CanvasGroup highlight;
    public Text highlightText;

    private Tween highlightTween;

    public PieceDesUIHandler desPanel;
    private InventoryHandler invenHandler;
    private BattleHandler bh;

    private bool isShow = false;

    [SerializeField] 
    private Text messageText;
    private string unusedMsg = "사용되지 않은 조각";
    private string usedMsg = "무덤에 있는 조각";

    private Action<SkillPiece> onClickPiece = null;
    private ShowInfoRange currentRange = ShowInfoRange.Inventory;

    public Action onCloseBtn = null;

    private void Awake()
    {
        PoolManager.CreatePool<PieceInfoUI>(pieceInfoObj, pieceHolderTrm, 5);

        GameManager.Instance.invenInfoHandler = this;
    }

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();

        startPos = cg.transform.localPosition.y;
        endPos = cg.transform.localPosition.y - rect.rect.height;

        contentRect = pieceHolderTrm.parent.GetComponent<RectTransform>();
        invenHandler = GameManager.Instance.inventoryHandler;
        bh = GameManager.Instance.battleHandler;

        invenBtn.onClick.AddListener(() =>
        {
            if (!isShow)
                ShowInventoryInfo(unusedMsg, ShowInfoRange.Inventory, desPanel.ShowDescription);
        });

        usedInvenBtn.onClick.AddListener(() => 
        {
            if (!isShow)
                ShowInventoryInfo(usedMsg, ShowInfoRange.Graveyard, desPanel.ShowDescription);
        });

        closeBtn.onClick.AddListener(() =>
        {
            CloseInventoryInfo();
        });

        invenHandler.onUpdateInfo += ResetInventoryInfo;

        ShowInfoPanel(false);
        cg.alpha = 0f;
    }

    public void ShowInventoryInfo(string msg, ShowInfoRange showRange, Action<SkillPiece> onClickPiece = null, Action onCloseBtn = null)
    {
        messageText.text = msg;

        this.onClickPiece = onClickPiece;
        this.onCloseBtn = onCloseBtn;

        currentRange = showRange;

        ShowInfoPanel(true);
        ResetInventoryInfo();
    }

    public void CloseInventoryInfo()
    {
        onCloseBtn?.Invoke();

        desPanel.ShowPanel(false);
        ShowInfoPanel(false);

        if (highlight.alpha > 0)
        {
            highlightTween.Kill();
            highlightTween = highlight.DOFade(0f, 0.33f);
        }
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

        List<SkillPiece> skills = null;

        switch (currentRange)
        {
            case ShowInfoRange.Inventory:
                skills = invenHandler.unusedSkills;
                break;

            case ShowInfoRange.Graveyard:
                skills = invenHandler.usedSkills;
                break;
        }

        for (int i = 0; i < skills.Count; i++)
        {
            SkillPiece sp = skills[i];

            PieceInfoUI pieceInfoUI = PoolManager.GetItem<PieceInfoUI>();
            pieceInfoUI.SetSkillIcon(sp.skillImg.sprite);

            pieceInfoUI.button.onClick.RemoveAllListeners();
            pieceInfoUI.button.onClick.AddListener(() =>
            {
                onClickPiece?.Invoke(sp);
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

    public void ShowHighlight(string message)
    {
        highlightText.text = message;

        highlightTween.Kill();
        highlightTween = highlight.DOFade(1f, 0.33f);
    }

    private void SetCGEnable(bool enable)
    {
        Time.timeScale = enable ? 0f : 1f;
        upCvs.sortingLayerName = enable ? ignoreEft : upUI;

        cg.alpha = 1f;
        cg.blocksRaycasts = enable;
        cg.interactable = enable;

        isShow = enable;
    }

    private void ShowCGEffect(bool enable)
    {
        if (enable)
        {
            bottomBG.ShowBottomPanel(false);

            cg.transform.DOLocalMoveY(startPos, 0.35f)
                .SetEase(Ease.OutBack, 0.7f)
                .SetUpdate(true);

            pauseCg.DOFade(1f, 0.2f)
                .SetUpdate(true);
        }
        else
        {
            bottomBG.ShowBottomPanel(true);

            cg.transform.DOLocalMoveY(endPos, 0.22f)
                .SetEase(Ease.OutCubic)
                .SetUpdate(true);

            pauseCg.DOFade(0f, 0.2f)
                .SetUpdate(true);
        }
    }
}
