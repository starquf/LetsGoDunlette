using DG.Tweening;
using System;
using UnityEngine;

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
                .Append(cvs.DOFade(1f, 0.45f).SetEase(Ease.Linear))
                .Append(startText.DOLocalMoveX(0f, 0.47f).SetEase(Ease.OutSine))
                .AppendInterval(0.25f)
                .Append(startText.DOLocalMoveX(-900f, 0.35f).SetEase(Ease.InQuad))
                .AppendCallback(() =>
                {
                    onEndShow?.Invoke();
                })
                .Append(cvs.DOFade(0f, 0.33f).SetEase(Ease.Linear))
                .SetUpdate(true);
    }
}
