using DG.Tweening;
using UnityEngine;

public abstract class BottomSwapUI : MonoBehaviour
{
    protected CanvasGroup cg;
    protected RectTransform rect;

    protected float startPos;
    protected float endPos;

    protected bool isShow = false;

    protected Sequence sizeSeq;

    protected virtual void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();

        startPos = cg.transform.localPosition.y;
        endPos = cg.transform.localPosition.y - rect.rect.height;

        ShowPanel(false);

        cg.alpha = 0f;
    }

    protected virtual void Start()
    {

    }

    public virtual void ClosePanel()
    {
        ShowPanel(false);
    }

    public virtual void ShowPanel(bool enable)
    {
        SetCGEnable(enable);
        ShowCGEffect(enable);
    }

    protected virtual void SetCGEnable(bool enable)
    {
        cg.alpha = 1f;
        cg.blocksRaycasts = enable;
        cg.interactable = enable;

        isShow = enable;
    }

    protected virtual void ShowCGEffect(bool enable)
    {
        if (enable)
        {
            cg.transform.DOLocalMoveY(startPos, 0.35f)
                .SetEase(Ease.OutBack, 0.7f)
                .SetUpdate(true);

            sizeSeq.Kill();
            sizeSeq = DOTween.Sequence()
                .Append(cg.transform.DOScale(Vector3.one * 1.06f, 0.12f))
                .Append(cg.transform.DOScale(Vector3.one, 0.12f));
        }
        else
        {
            cg.transform.DOLocalMoveY(endPos, 0.22f)
                .SetEase(Ease.OutCubic)
                .SetUpdate(true);

            cg.transform.localScale = Vector3.one;
        }
    }
}
