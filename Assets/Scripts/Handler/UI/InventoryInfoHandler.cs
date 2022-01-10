using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryInfoHandler : MonoBehaviour
{
    private CanvasGroup cg;
    private RectTransform contentRect;

    [Header("¹öÆ°")]
    public Button invenBtn;
    public Button usedInvenBtn;
    public Button closeBtn;

    [Header("Á¶°¢ È¦´õ °ü·Ã")]
    public Transform pieceHolderTrm;
    public GameObject pieceInfoObj;

    public PieceDesUIHandler desPanel;

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

            PieceInfoUI pieceInfoUI = PoolManager.GetItem<PieceInfoUI>();
            pieceInfoUI.SetSkillIcon(icon);

            pieceInfoUI.button.onClick.RemoveAllListeners();
            pieceInfoUI.button.onClick.AddListener(() =>
            {
                desPanel.ShowDescription(name, icon, "¼³¸í Àû±â ±ÍÂú´Ù");
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
    }
}
