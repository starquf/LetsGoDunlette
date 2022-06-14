using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomUIHandler : MonoBehaviour
{
    private CanvasGroup cg;

    private float startPos;
    private float downPos;

    public bool isShow = true;

    private void Awake()
    {
        GameManager.Instance.bottomUIHandler = this;
        cg = GetComponent<CanvasGroup>();

        startPos = transform.localPosition.y;
        downPos = transform.localPosition.y - 240f;
    }

    public void ShowBottomPanel(bool enable)
    {
        isShow = enable;
        cg.interactable = enable;
        cg.blocksRaycasts = enable;

        if (enable)
        {
            transform.DOLocalMoveY(startPos, 0.33f)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);

            transform.parent.SetAsLastSibling();
        }
        else
        {
            transform.DOLocalMoveY(downPos, 0.33f)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);

            transform.parent.SetAsFirstSibling();
        }
    }
}
