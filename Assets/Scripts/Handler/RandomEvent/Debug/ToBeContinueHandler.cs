using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class ToBeContinueHandler : MonoBehaviour
{
    public CanvasGroup panel;
    public Text timerText;

    private void Awake()
    {
        ShowPanel(false);
    }

    public void StartEvent()
    {
        ShowPanel(true);

        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        for (int i = 5; i > 0; i--)
        {
            timerText.text = $"{i}초후에 맵으로 돌아갑니다...";
            yield return new WaitForSeconds(1f);
        }

        EndEvent();
    }

    private void EndEvent()
    {
        ShowPanel(false);

        GameManager.Instance.EndEncounter();
    }

    private void ShowPanel(bool enable)
    {
        if (enable)
        {
            panel.DOFade(1f, 0.5f);
        }
        else
        {
            panel.alpha = 0f;
        }

        panel.blocksRaycasts = enable;
        panel.interactable = enable;
    }
}
