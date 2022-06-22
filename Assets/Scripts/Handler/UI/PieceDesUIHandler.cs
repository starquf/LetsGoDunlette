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

        if (confirmBtn != null)
        {
            confirmBtn.gameObject.SetActive(false);
        }

        ShowPanel(true);
    }

    private void ShowDesIcon(List<DesIconInfo> desInfos, SkillPiece skillPiece)
    {
        int count = 0;
        skillIconTrans.gameObject.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            DesIconType type = desInfos[i].iconType;

            if (type.Equals(DesIconType.None))
            {
                count++;
                desIcons[i].gameObject.SetActive(false);
                continue;
            }
            else
            {
                desIcons[i].gameObject.SetActive(true);
            }

            Sprite icon = GetDesIcon(skillPiece, type);

            desIcons[i].SetIcon(icon, desInfos[i].value);
        }

        if (count >= 3)
        {
            skillIconTrans.gameObject.SetActive(false);
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

    public Sprite GetDesIcon(SkillPiece skillPiece, DesIconType type)
    {
        Sprite icon = null;

        icon = type switch
        {
            DesIconType.Attack => GameManager.Instance.inventoryHandler.effectSprDic[skillPiece.currentType],
            DesIconType.Stun => GameManager.Instance.ccIcons[0],
            DesIconType.Silence => GameManager.Instance.ccIcons[1],
            DesIconType.Exhausted => GameManager.Instance.ccIcons[2],
            DesIconType.Wound => GameManager.Instance.ccIcons[3],
            DesIconType.Invincibility => GameManager.Instance.ccIcons[4],
            DesIconType.Fascinate => GameManager.Instance.ccIcons[5],
            DesIconType.Heating => GameManager.Instance.ccIcons[6],
            DesIconType.Shield => GameManager.Instance.buffIcons[0],
            DesIconType.Heal => GameManager.Instance.buffIcons[1],
            DesIconType.Upgrade => GameManager.Instance.buffIcons[2],
            _ => null,
        };
        return icon;
    }
}
