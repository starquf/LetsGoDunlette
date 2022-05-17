using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomUIHandler : MonoBehaviour
{
    private CanvasGroup cg;

    public Button swapBtn;

    private float startPos;
    private float downPos;

    public bool isShow = true;

    public List<BottomSwapUI> swapUIs = new List<BottomSwapUI>();
    private int swapUIIdx = 0;

    private void Awake()
    {
        GameManager.Instance.bottomUIHandler = this;
        cg = GetComponent<CanvasGroup>();

        startPos = transform.localPosition.y;
        downPos = transform.localPosition.y - 240f;
    }

    private void Start()
    {
        ShowSwapUI(0);

        swapBtn.onClick.AddListener(() =>
        {
            SwapToNextUI();
        });
    }

    public void SwapToNextUI()
    {
        swapUIIdx = (swapUIIdx + 1) % swapUIs.Count;

        ShowSwapUI(swapUIIdx);
    }

    protected void ShowSwapUI(int idx)
    {
        if (idx >= swapUIs.Count)
        {
            return;
        }

        for (int i = 0; i < swapUIs.Count; i++)
        {
            swapUIs[i].ShowPanel(false);
        }

        swapUIs[idx].ShowPanel(true);

        swapUIs[idx].transform.SetAsLastSibling();
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
