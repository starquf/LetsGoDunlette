using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class BattleFadeUIHandler : MonoBehaviour
{
    private CanvasGroup cvs;
    public Transform startText;

    private void Start()
    {
        cvs = GetComponent<CanvasGroup>();

        cvs.alpha = 0f;
        cvs.interactable = false;
        cvs.blocksRaycasts = false;
    }

    public void ShowEffect(Action onEndShow = null)
    {
        startText.localPosition = new Vector3(900f, startText.localPosition.y, 0f);

        Sequence startSeq = DOTween.Sequence()
                .Append(cvs.DOFade(1f, 0.39f).SetEase(Ease.Linear))
                .Append(startText.DOLocalMoveX(0f, 0.7f).SetEase(Ease.OutSine))
                //.AppendInterval(0.1f)
                .Append(startText.DOLocalMoveX(-900f, 0.5f).SetEase(Ease.InQuad))
                .AppendCallback(() =>
                {
                    onEndShow?.Invoke();
                })
                .Append(cvs.DOFade(0f, 0.33f).SetEase(Ease.Linear))
                .SetUpdate(true);
    }
}
