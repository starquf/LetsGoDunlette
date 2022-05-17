using UnityEngine;
using UnityEngine.UI;

public class UISettingPanel : BottomUIElement
{
    public Button settingButton;

    public Button closeBtn;
    public Button closeImgBtn;

    protected override void Start()
    {
        base.Start();

        settingButton.onClick.AddListener(() =>
        {
            if (Time.timeScale <= 0)
            {
                return;
            }

            if (!isShow)
            {
                Popup();
            }
        });

        closeBtn.onClick.AddListener(() =>
        {
            ClosePanel();
        });

        closeImgBtn.onClick.AddListener(() =>
        {
            ClosePanel();
        });
    }
}
