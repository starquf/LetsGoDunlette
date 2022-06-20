using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : MainUIPanel
{
    public Button tapBtn;

    public MainUIPanel lobbyPanel;

    protected void Start()
    {
        tapBtn.onClick.AddListener(() =>
        {
            mainUIHandler.ChangePanel(this, lobbyPanel);
        });
    }
}
