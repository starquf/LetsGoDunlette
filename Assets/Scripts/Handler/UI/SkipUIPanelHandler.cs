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

    private void Start()
    {
        SetPanel(false);
    }

    public void ShowSkipUI()
    {
        SetPanel(true);

        skipText.enabled = true;
        skipTextTween = skipText.DOFade(1f, 0.8f)
                        .SetEase(Ease.Linear)
                        .SetLoops(-1, LoopType.Yoyo)
                        .From(0f);
    }

    public void ShowDragUI()
    {
        
    }

    public void SetPanel(bool enable)
    {
        cg.alpha = enable ? 1 : 0;

        if(!enable)
        {
            print("²¨Áü");

            skipTextTween.Kill();
            skipText.enabled = false;
        }
    }
}
