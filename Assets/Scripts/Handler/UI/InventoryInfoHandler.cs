using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryInfoHandler : MonoBehaviour
{
    private CanvasGroup cg;
    private RectTransform contentRect;

    [Header("버튼")]
    public Button invenBtn;
    public Button usedInvenBtn;
    public Button closeBtn;

    [Header("조각 홀더 관련")]
    public Transform pieceHolderTrm;
    public GameObject pieceInfoObj;

    private InventoryHandler invenHandler;

    private void Awake()
    {
        PoolManager.CreatePool<PieceInfoUI>(pieceInfoObj, pieceHolderTrm, 5);
    }

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        contentRect = pieceHolderTrm.parent.GetComponent<RectTransform>();
        invenHandler = GameManager.Instance.inventoryHandler;

        invenBtn.onClick.AddListener(() => { ShowInventoryInfo(); });
        usedInvenBtn.onClick.AddListener(() => { ShowInfoPanel(true); });

        closeBtn.onClick.AddListener(() => ShowInfoPanel(false));

        ShowInfoPanel(false);
    }

    public void ShowInventoryInfo()
    {
        ResetPieceInfo();

        List<SkillPiece> skills = invenHandler.unusedSkills;

        for (int i = 0; i < skills.Count; i++)
        {
            PieceInfoUI pieceInfoUI = PoolManager.GetItem<PieceInfoUI>();
            pieceInfoUI.SetSkillIcon(skills[i].skillImg.sprite);
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
    }
}
