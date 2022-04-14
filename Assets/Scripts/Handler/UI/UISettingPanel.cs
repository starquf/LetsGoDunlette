using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingPanel : MonoBehaviour
{
    public Button settingButton;

    private CanvasGroup cg;
    private RectTransform rect;

    public Canvas upCvs;
    private string ignoreEft = "IgnoreEffect";
    private string upUI = "UpUI";

    public CanvasGroup pauseCg;

    public BottomUIHandler bottomBG;

    private float startPos;
    private float endPos;

    private bool stopTime;
    private bool isShow;

    public Button closeBtn;
    public Button closeImgBtn;

    public CanvasGroup highlight;
    private Tween highlightTween;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();

        startPos = cg.transform.localPosition.y;
        endPos = cg.transform.localPosition.y - rect.rect.height;

        settingButton.onClick.AddListener(() =>
        {
            if (Time.timeScale <= 0) return;

            if (!isShow)
                Popup();
        });

        closeBtn.onClick.AddListener(() =>
        {
            ClosePanel();
        });

        closeImgBtn.onClick.AddListener(() =>
        {
            ClosePanel();
        });

        ShowInfoPanel(false);

        cg.alpha = 0f;
    }

    public void ClosePanel()
    {
        //desPanel.ShowPanel(false);
        ShowInfoPanel(false);

        if (highlight.alpha > 0)
        {
            highlightTween.Kill();
            highlightTween = highlight.DOFade(0f, 0.33f);
        }
    }

    public void Popup(bool stopTime = true)
    {
        this.stopTime = stopTime;

        ShowInfoPanel(true);
        ResetInventoryInfo();
    }

    private void ResetInventoryInfo()
    {
        //throw new NotImplementedException();
    }

    private void ShowInfoPanel(bool enable)
    {
        SetCGEnable(enable);
        ShowCGEffect(enable);
    }

    private void SetCGEnable(bool enable)
    {
        if (stopTime)
        {
            Time.timeScale = enable ? 0f : 1f;
        }

        upCvs.sortingLayerName = enable ? ignoreEft : upUI;

        cg.alpha = 1f;
        cg.blocksRaycasts = enable;
        cg.interactable = enable;

        isShow = enable;
    }

    private void ShowCGEffect(bool enable)
    {
        if (enable)
        {
            bottomBG.ShowBottomPanel(false);

            cg.transform.DOLocalMoveY(startPos, 0.35f)
                .SetEase(Ease.OutBack, 0.7f)
                .SetUpdate(true);

            if (stopTime)
            {
                pauseCg.DOFade(1f, 0.2f)
                    .SetUpdate(true);
            }
        }
        else
        {
            bottomBG.ShowBottomPanel(true);

            cg.transform.DOLocalMoveY(endPos, 0.22f)
                .SetEase(Ease.OutCubic)
                .SetUpdate(true);

            pauseCg.DOFade(0f, 0.2f)
                .SetUpdate(true);
        }
    }
}
