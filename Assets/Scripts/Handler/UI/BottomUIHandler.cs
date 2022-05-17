using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomUIHandler : MonoBehaviour
{
    private CanvasGroup cg;

    public Button swapBtn;
    private Image swapIcon;

    private float startPos;
    private float downPos;

    public bool isShow = true;

    public List<BottomSwapUI> swapUIs = new List<BottomSwapUI>();
    public List<Sprite> swapIconList = new List<Sprite>();

    private int swapUIIdx = 0;

    private void Awake()
    {
        swapIcon = swapBtn.transform.GetChild(0).GetComponent<Image>();
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
            if (Time.timeScale < 1f)
                return;

            SwapToNextUI();
        });
    }

    public void SwapToNextUI()
    {
        swapUIIdx = (swapUIIdx + 1) % swapUIs.Count;

        swapIcon.sprite = swapIconList[swapUIIdx];
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
