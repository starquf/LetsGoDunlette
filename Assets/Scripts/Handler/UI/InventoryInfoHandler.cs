using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryInfoHandler : BottomUIElement
{
    private RectTransform contentRect;

    [Header("��ư")]
    public Button invenBtn;
    public Button usedInvenBtn;
    public Button closeBtn;
    public Button closeImgBtn;

    [Header("���� Ȧ�� ����")]
    public Transform pieceHolderTrm;
    public GameObject pieceInfoObj;

    [Header("���̶���Ʈ ����")]
    public CanvasGroup highlight;
    public Text highlightText;

    private Tween highlightTween;

    public PieceDesUIHandler desPanel;
    private InventoryHandler invenHandler;
    private BattleHandler bh;

    [SerializeField]
    private Text messageText;
    private string unusedMsg = "������ ���� ����";
    private string usedMsg = "������ �ִ� ����";

    private Action<SkillPiece> onClickPiece = null;
    private ShowInfoRange currentRange = ShowInfoRange.Inventory;

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
                ShowInventoryInfo(unusedMsg, ShowInfoRange.Inventory, desPanel.ShowDescription);
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

        currentRange = showRange;

        ShowPanel(true);
        ResetInventoryInfo();
    }

    private void CloseInventoryInfoHasAction()
    {
        onCloseBtn?.Invoke();

        if (closePanel)
        {
            desPanel.ShowPanel(false);
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

        List<SkillPiece> skills = null;

        switch (currentRange)
        {
            case ShowInfoRange.Inventory:
                skills = bh.player.GetComponent<Inventory>().skills;
                break;

            case ShowInfoRange.Graveyard:
                skills = invenHandler.graveyard;
                break;
        }

        for (int i = 0; i < skills.Count; i++)
        {
            SkillPiece sp = skills[i];

            PieceInfoUI pieceInfoUI = PoolManager.GetItem<PieceInfoUI>();
            pieceInfoUI.SetSkillIcon(sp.skillIconImg.sprite, sp.skillStroke);
            pieceInfoUI.transform.SetParent(pieceHolderTrm);
            pieceInfoUI.GetComponent<Image>().color = Color.white;

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
