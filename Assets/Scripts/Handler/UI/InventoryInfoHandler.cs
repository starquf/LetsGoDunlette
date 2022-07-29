using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryInfoHandler : BottomUIElement
{
    private RectTransform contentRect;

    [Header("버튼")]
    public Button invenBtn;
    public Button usedInvenBtn;
    public Button closeBtn;
    public Button closeImgBtn;

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

    [SerializeField]
    private Text messageText;
    private string unusedMsg = "인벤토리";
    private string usedMsg = "무덤에 있는 조각";

    private Action<SkillPiece> onClickPiece = null;
    private List<SkillPiece> currentShowPieces = new List<SkillPiece>();

    public Action onCloseBtn = null;
    private bool closePanel = true;

    private void Awake()
    {
        PoolManager.CreatePool<PieceInfoUI>(pieceInfoObj, pieceHolderTrm, 5);

        GameManager.Instance.invenInfoHandler = this;
    }

    protected override void Start()
    {
        base.Start();

        contentRect = pieceHolderTrm.parent.GetComponent<RectTransform>();
        invenHandler = GameManager.Instance.inventoryHandler;
        bh = GameManager.Instance.battleHandler;

        invenBtn.onClick.AddListener(() =>
        {
            if (Time.timeScale < 1f)
            {
                return;
            }

            if (!isShow)
            {
                int count = bh.player.GetComponent<Inventory>().skills.Count;
                int maxCount = bh.player.MaxPieceCount;

                ShowInventoryInfo($"{unusedMsg}({count}/{maxCount})", ShowInfoRange.Inventory, desPanel.ShowDescription);
            }
        });

        usedInvenBtn.onClick.AddListener(() =>
        {
            if (Time.timeScale < 1f)
            {
                return;
            }

            if (!isShow)
            {
                ShowInventoryInfo(usedMsg, ShowInfoRange.Graveyard, desPanel.ShowDescription);
            }
        });

        closeBtn.onClick.AddListener(() =>
        {
            CloseInventoryInfoHasAction();
        });

        closeImgBtn.onClick.AddListener(() =>
        {
            CloseInventoryInfoHasAction();
        });

        invenHandler.onUpdateInfo += ResetInventoryInfo;
    }

    public void ShowInventoryInfo(string msg, ShowInfoRange showRange, Action<SkillPiece> onClickPiece = null, Action onCloseBtn = null, bool stopTime = true, bool closePanel = true)
    {
        this.stopTime = stopTime;

        messageText.text = msg;

        this.onClickPiece = onClickPiece;
        this.onCloseBtn = onCloseBtn;
        this.closePanel = closePanel;

        switch (showRange)
        {
            case ShowInfoRange.Inventory:
                currentShowPieces = bh.player.GetComponent<Inventory>().skills;
                break;

            case ShowInfoRange.Graveyard:
                currentShowPieces = invenHandler.graveyard;
                break;

            default:
                currentShowPieces = new List<SkillPiece>();
                break;
        }

        ShowPanel(true);
        ResetInventoryInfo();
    }

    public void ShowInventoryInfo(string msg, List<SkillPiece> showPieces, Action<SkillPiece> onClickPiece = null, Action onCloseBtn = null, bool stopTime = true, bool closePanel = true)
    {
        this.stopTime = stopTime;

        messageText.text = msg;

        this.onClickPiece = onClickPiece;
        this.onCloseBtn = onCloseBtn;
        this.closePanel = closePanel;

        currentShowPieces = showPieces;

        ShowPanel(true);
        ResetInventoryInfo();
    }

    private void CloseInventoryInfoHasAction()
    {
        onCloseBtn?.Invoke();

        if (closePanel)
        {
            desPanel.ShowPanel(false);
            desPanel.iconInfoHandler.ClosePanel();
            ShowPanel(false);

            if (highlight.alpha > 0)
            {
                highlightTween.Kill();
                highlightTween = highlight.DOFade(0f, 0.33f);
            }
        }
    }

    public void CloseInventoryInfo()
    {
        desPanel.ShowPanel(false);
        desPanel.iconInfoHandler.ClosePanel();
        ShowPanel(false);

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
        if (!isShow)
        {
            return;
        }

        ResetPieceInfo();

        for (int i = 0; i < currentShowPieces.Count; i++)
        {
            SkillPiece sp = currentShowPieces[i];

            PieceInfoUI pieceInfoUI = PoolManager.GetItem<PieceInfoUI>();
            pieceInfoUI.SetSkillIcon(sp.skillIconImg.sprite, sp.skillStroke);
            pieceInfoUI.transform.SetParent(pieceHolderTrm);

            pieceInfoUI.button.onClick.RemoveAllListeners();
            pieceInfoUI.button.onClick.AddListener(() =>
            {
                onClickPiece?.Invoke(sp);
            });

            pieceInfoUI.transform.SetAsFirstSibling();
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
    }

    public void ShowHighlight(string message)
    {
        highlightText.text = message;

        highlightTween.Kill();
        highlightTween = highlight.DOFade(1f, 0.33f)
            .SetUpdate(true);
    }
}
