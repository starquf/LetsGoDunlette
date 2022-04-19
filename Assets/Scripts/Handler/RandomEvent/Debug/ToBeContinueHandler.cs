using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class ToBeContinueHandler : MonoBehaviour
{
    private CanvasGroup panel;
    public Text timerText;

    private Action onEndToBeContinue;
    private string endActionString;

    private void Awake()
    {
        GameManager.Instance.tbcHandler = this;
        panel = GetComponent<CanvasGroup>();
        ShowPanel(false, true);
    }

    public void StartEvent(Action onEndToBeContinue, string str)
    {
        this.onEndToBeContinue = onEndToBeContinue;
        this.endActionString = str;
        ShowPanel(true);

        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        for (int i = 5; i > 0; i--)
        {
            timerText.text = $"{i}���Ŀ� {endActionString}...";
            yield return new WaitForSeconds(1f);
        }

        EndEvent();
    }

    private void EndEvent()
    {
        ShowPanel(false, onComplete : ()=> {
            onEndToBeContinue?.Invoke();
            this.onEndToBeContinue = null;
            this.endActionString = null;
        });
    }

    private void ShowPanel(bool enable, bool skip = false, Action onComplete = null)
    {
        if(skip)
        {
            panel.alpha = enable ? 1f : 0f;
            onComplete?.Invoke();
        }
        else
        {
            panel.DOFade(enable?1f:0f, 0.5f).OnComplete(()=>onComplete?.Invoke());
        }

        panel.blocksRaycasts = enable;
        panel.interactable = enable;
    }
}
