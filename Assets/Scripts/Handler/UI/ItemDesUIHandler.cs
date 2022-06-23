using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDesUIHandler : MonoBehaviour
{
    private CanvasGroup cg;

    [Header("설명들")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI desText;
    public Image iconImg;

    [Header("버튼들")]
    public Button confirmBtn;
    public Button cancelBtn;
    public Text confirmBtnText;

    [Space(10f)]
    public CanvasGroup blackPanel;
    public CanvasGroup blackoutPanel;

    public bool isShow = false;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();

        ShowCG(false);
    }

    public void ShowDes(string name, string des, Sprite icon, string confirmBtnStr, Action onConfirm = null, Action onCancel = null)
    {
        ShowCG(true);

        nameText.text = name;
        desText.text = des;
        iconImg.sprite = icon;
        confirmBtnText.text = confirmBtnStr;

        confirmBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();

        confirmBtn.onClick.AddListener(() =>
        {
            ShowCG(false);
            onConfirm?.Invoke();
        });

        cancelBtn.onClick.AddListener(() =>
        {
            ShowCG(false);
            onCancel?.Invoke();
        });
    }

    public void ForceCancelDes()
    {
        ShowCG(false);

        confirmBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();
    }

    private void ShowCG(bool enable)
    {
        cg.alpha = enable ? 1f : 0f;
        cg.interactable = enable;
        cg.blocksRaycasts = enable;

        blackPanel.alpha = enable ? 1f : 0f;
        blackoutPanel.alpha = enable ? 1f : 0f;

        isShow = enable;
    }
}
