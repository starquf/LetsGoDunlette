using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIHandler : MonoBehaviour
{
    public CanvasGroup fadeBG;
    public MainUIPanel startPanel;

    public List<MainUIPanel> mainUIs = new List<MainUIPanel>();

    private void Start()
    {
        GameManager.Instance.SetResolution();

        InitPanel();

        startPanel.ShowPanel(true);

        SetFade(true, true);
        SetFade(false, false, () => 
        {
            startPanel.SetInteract(true);
        });
    }

    private void InitPanel()
    {
        for (int i = 0; i < mainUIs.Count; i++)
        {
            mainUIs[i].Init(this);
        }
    }

    public void ChangePanel(MainUIPanel before, MainUIPanel after)
    {
        before.SetInteract(false);

        SetFade(true, onFinishFade: () =>
        {
            before.ShowPanel(false);
            after.ShowPanel(true);

            SetFade(false, onFinishFade: () => 
            {
                after.SetInteract(true);
            });
        });
    }

    public void SetFade(bool isFade, bool isSkip = false, Action onFinishFade = null)
    {
        if (isSkip)
        {
            fadeBG.alpha = isFade ? 1f : 0f;
            onFinishFade?.Invoke();
        }
        else 
        {
            fadeBG.DOFade(isFade ? 1f : 0f, 0.75f)
                .SetEase(Ease.Linear)
                .OnComplete(() => onFinishFade?.Invoke());
        }
    }
}
