using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PieceCastUIHandler : MonoBehaviour
{
    public Image iconImg;

    private CanvasGroup cvsGroup;
    private Sequence showSequence;

    void Awake()
    {
        cvsGroup = GetComponent<CanvasGroup>();

        ShowPanel(false, true);
    }

    public void ShowCasting(Sprite icon)
    {
        iconImg.sprite = icon;

        ShowPanel(true);
    }

    public void ShowPanel(bool enable, bool skip = false)
    {
        showSequence.Kill();
        if (!skip)
        {
            showSequence.Append(cvsGroup.DOFade(enable ? 1 : 0, enable ? 0.2f:0.5f).OnComplete(() => {
                cvsGroup.interactable = enable;
                cvsGroup.blocksRaycasts = enable;
            }));
        }
        else
        {
            cvsGroup.alpha = enable ? 1 : 0;
            cvsGroup.interactable = enable;
            cvsGroup.blocksRaycasts = enable;
        }
    }
}
