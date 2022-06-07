using System;
using UnityEngine;
using UnityEngine.UI;

public class YesOrNoPanel : MonoBehaviour
{
    private CanvasGroup YONPanel = null;
    public Button cancelBtn = null;
    public Button confirmBtn = null;

    private Action onConfirmBtn = null;
    private Action onCancelBtn = null;


    public Text panelDestTxt = null;
    public Text cancelTxt = null;
    public Text confirmTxt = null;

    private void Awake()
    {
        GameManager.Instance.YONHandler = this;
        YONPanel = GetComponent<CanvasGroup>();
        Show(false);
    }

    private void Start()
    {
        cancelBtn.onClick.AddListener(() =>
        {
            onCancelBtn?.Invoke();
            Show(false);
        });

        confirmBtn.onClick.AddListener(() =>
        {
            onConfirmBtn?.Invoke();
            Show(false);
        });
    }

    public void ShowPanel(string des, string confirmBtnText, string cancelBtnText, Action onConfirmBtn = null, Action onCancelBtn = null)
    {
        panelDestTxt.text = des;
        cancelTxt.text = cancelBtnText;
        confirmTxt.text = confirmBtnText;

        this.onCancelBtn = onCancelBtn;
        this.onConfirmBtn = onConfirmBtn;

        Show();
    }

    private void Show(bool enable = true)
    {
        YONPanel.alpha = enable ? 1 : 0;
        YONPanel.blocksRaycasts = enable;
        YONPanel.interactable = enable;
    }
}
