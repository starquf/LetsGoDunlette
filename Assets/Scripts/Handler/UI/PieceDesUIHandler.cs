using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PieceDesUIHandler : MonoBehaviour
{
    private InventoryHandler invenHandler;

    public TextMeshProUGUI nameText;
    public Image bgImg;
    public TextMeshProUGUI desText;
    public Transform skillIconTrans;
    public Image strokeImg;
    public Image targetBGImg;
    public Image targetImg;
    public GradeInfoHandler gradeHandler;

    private List<SkillDesIcon> desIcons = new List<SkillDesIcon>();

    [Space(10f)]
    public Button closeBtn;
    public Button confirmBtn;

    private CanvasGroup cg;
    private RectTransform rect;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();

        skillIconTrans.GetComponentsInChildren(desIcons);

        invenHandler = GameManager.Instance.inventoryHandler;

        closeBtn.onClick.AddListener(() => ShowPanel(false));

        ShowPanel(false);
    }

    public void ShowDescription(SkillPiece skillPiece)
    {
        Sprite bg = skillPiece.cardBG;
        Sprite stroke = invenHandler.pieceBGStrokeSprDic[skillPiece.currentType];
        Sprite targetBG = invenHandler.targetBGSprDic[skillPiece.currentType];
        Sprite targetIcon = invenHandler.targetIconSprDic[skillPiece.skillRange];
        string name = skillPiece.PieceName;
        string des = skillPiece.PieceDes;

        nameText.text = name;
        bgImg.sprite = bg;
        desText.text = des;
        strokeImg.sprite = stroke;
        targetBGImg.sprite = targetBG;
        targetImg.sprite = targetIcon;
        gradeHandler.SetGrade(skillPiece.skillGrade);

        List<DesIconInfo> desInfos = skillPiece.GetDesIconInfo();
        ShowDesIcon(desInfos, skillPiece);

        if (skillPiece.PieceDes.Equals(""))
        {
            desText.gameObject.SetActive(false);
        }
        else
        {
            desText.gameObject.SetActive(true);
        }

        confirmBtn.gameObject.SetActive(false);

        ShowPanel(true);
    }

    private void ShowDesIcon(List<DesIconInfo> desInfos, SkillPiece skillPiece)
    {
        for (int i = 0; i < 3; i++)
        {
            DesIconType type = desInfos[i].iconType;

            if (type.Equals(DesIconType.None))
            {
                desIcons[i].gameObject.SetActive(false);
                continue;
            }
            else
            {
                desIcons[i].gameObject.SetActive(true);
            }

            Sprite icon = GameManager.Instance.battleHandler.battleUtil.GetDesIcon(skillPiece, type);

            desIcons[i].SetIcon(icon, desInfos[i].value);
        }
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
