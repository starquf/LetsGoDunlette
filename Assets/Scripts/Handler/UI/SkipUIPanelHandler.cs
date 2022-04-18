using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipUIPanelHandler : MonoBehaviour
{
    public CanvasGroup cg;

    public Text skipText;
    private Tween skipTextTween;

    public Image fingerImg;
    private Vector3 fingerStartPos;
    private Sequence fingerSeq;

    private bool isShow = false;

    private void Start()
    {
        SetPanel(false);

        fingerStartPos = fingerImg.transform.localPosition;
    }

    public void ShowSkipUI()
    {
        if (isShow) return;

        isShow = true;

        SetPanel(true);

        skipText.enabled = true;
        skipText.text = "터치로 스킵";

        skipTextTween = skipText.DOFade(1f, 0.8f)
                        .SetEase(Ease.Linear)
                        .SetLoops(-1, LoopType.Yoyo)
                        .From(0f);
    }

    public void ShowDragUI()
    {
        if (isShow) return;

        isShow = true;

        SetPanel(true);

        fingerImg.transform.localPosition = fingerStartPos;

        fingerImg.color = new Color(1f, 1f, 1f, 0f);
        fingerImg.enabled = true;
        
        fingerSeq = DOTween.Sequence()
            .AppendCallback(() =>
            {
                fingerImg.color = new Color(1f, 1f, 1f, 0f);
            })
            .Append(fingerImg.DOFade(1f, 0.6f))
            .Append(fingerImg.transform.DOLocalMoveY(400f, 0.6f))
            .AppendCallback(() =>
            {
                fingerImg.color = Color.white;
            })
            .Append(fingerImg.DOFade(0f, 0.6f))
            .AppendCallback(() =>
            {
                fingerImg.transform.localPosition = fingerStartPos;
            })
            .AppendInterval(0.6f)
            .SetLoops(-1);

        skipText.enabled = true;
        skipText.text = "적에게 드래그하세요";

        skipTextTween = skipText.DOFade(1f, 1f)
                        .SetEase(Ease.Linear)
                        .SetLoops(-1, LoopType.Yoyo)
                        .From(0f);
    }

    public void SetPanel(bool enable)
    {
        cg.alpha = enable ? 1 : 0;

        if(!enable)
        {
            isShow = false;

            skipTextTween.Kill();
            skipText.enabled = false;

            fingerSeq.Kill();
            fingerImg.enabled = false;
        }
    }
}
