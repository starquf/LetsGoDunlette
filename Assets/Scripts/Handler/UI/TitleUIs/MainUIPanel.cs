using UnityEngine;

public abstract class MainUIPanel : MonoBehaviour
{
    protected CanvasGroup cg;

    protected MainUIHandler mainUIHandler;

    protected virtual void Awake()
    {
        cg = GetComponent<CanvasGroup>();

        ShowPanel(false);
        SetInteract(false);
    }

    public virtual void Init(MainUIHandler mainUI)
    {
        mainUIHandler = mainUI;
    }

    public virtual void ShowPanel(bool enable)
    {
        cg.alpha = enable ? 1f : 0f;
    }

    public virtual void SetInteract(bool enable)
    {
        cg.blocksRaycasts = enable;
        cg.interactable = enable;
    }
}
