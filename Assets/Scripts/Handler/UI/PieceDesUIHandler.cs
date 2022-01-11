using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceDesUIHandler : MonoBehaviour
{
    public Text nameText;
    public Image iconImg;
    public Text desText;

    [Space(10f)]
    public Button closeBtn;

    private CanvasGroup cg;
    private RectTransform rect;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();

        closeBtn.onClick.AddListener(() => ShowPanel(false));

        ShowPanel(false);
    }

    public void ShowDescription(string name, Sprite icon, string des)
    {
        nameText.text = name;
        iconImg.sprite = icon;
        desText.text = des;

        ShowPanel(true);
    }

    public void ShowPanel(bool enable)
    {
        cg.alpha = enable ? 1f : 0f;
        cg.blocksRaycasts = enable;
        cg.interactable = enable;
    }
}