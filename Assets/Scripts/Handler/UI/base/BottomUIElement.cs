using DG.Tweening;
using UnityEngine;

public abstract class BottomUIElement : MonoBehaviour
{
    protected CanvasGroup cg;
    protected RectTransform rect;

    public Canvas upCvs;
    protected string ignoreEft = "IgnoreEffect";
    protected string upUI = "UpUI";

    public CanvasGroup pauseCg;

    public BottomUIHandler bottomBG;

    protected float startPos;
    protected float endPos;

    protected bool stopTime;

    protected bool isShow = false;

    public bool canControl = true;

    protected virtual void Start()
    {
        cg = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();

        startPos = cg.transform.localPosition.y;
        endPos = cg.transform.localPosition.y - rect.rect.height;

        ShowPanel(false);

        cg.alpha = 0f;
    }

    public virtual void ClosePanel()
    {
        ShowPanel(false);
    }

    public virtual void Popup(bool stopTime = true)
    {
        this.stopTime = stopTime;

        ShowPanel(true);
    }

    protected virtual void ShowPanel(bool enable)
    {
        if (!canControl)
            return;

        SetCGEnable(enable);
        ShowCGEffect(enable);
    }

    protected virtual void SetCGEnable(bool enable)
    {
        if (stopTime && GameManager.Instance.battleHandler.isBattleStart)
        {
            Time.timeScale = enable ? 0f : 1f;
        }

        upCvs.sortingLayerName = enable ? ignoreEft : upUI;

        cg.alpha = 1f;
        cg.blocksRaycasts = enable;
        cg.interactable = enable;

        isShow = enable;
    }

    protected virtual void ShowCGEffect(bool enable)
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
