using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollDesUIHandler : MonoBehaviour
{
    private CanvasGroup cg;

    [Header("설명들")]
    public Text nameText;
    public Text desText;
    public Image iconImg;

    [Header("버튼들")]
    public Button confirmBtn;
    public Button cancelBtn;
    public Text confirmBtnText;

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

    private void ShowCG(bool enable)
    {
        cg.alpha = enable ? 1f : 0f;
        cg.interactable = enable;
        cg.blocksRaycasts = enable;
    }
}
