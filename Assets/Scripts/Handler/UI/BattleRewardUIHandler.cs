using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleRewardUIHandler : MonoBehaviour
{
    private CanvasGroup allCvsGroup;
    [SerializeField] private Transform battleWinTextImgTrm;
    [SerializeField] private CanvasGroup rewardCvsGroup;
    [SerializeField] private Button rewardBtn;
    [SerializeField] private Button rewardSkipBtn;
    [SerializeField] private Text rewardBtnText;

    private Sequence showSequence;
    private Sequence winShowSequence;

    private void Awake()
    {
        allCvsGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        rewardBtn.onClick.AddListener(() => {
            print("tlqkf");
        });
        rewardSkipBtn.onClick.AddListener(() => {
            print("whfrpxsp");
        });
    }

    public void SetButton(Action onClickGet, Action onClickSkip)
    {
        rewardBtn.onClick.RemoveAllListeners();
        rewardSkipBtn.onClick.RemoveAllListeners();

        rewardBtnText.text = "³Ñ±â±â";

        rewardBtn.onClick.AddListener(() => {
            onClickGet();
            ShowPanel(allCvsGroup, false, ()=> {
                ShowPanel(rewardCvsGroup, false, null, true);
            });
            AllBtnHandle(false);
        });
        rewardSkipBtn.onClick.AddListener(() => {
            onClickSkip();
            ShowPanel(allCvsGroup, false, () => {
                ShowPanel(rewardCvsGroup, false, null, true);
            });
            AllBtnHandle(false);
        });
        AllBtnHandle(true);
    }

    public void AllBtnHandle(bool open)
    {
        rewardSkipBtn.gameObject.SetActive(open);
        rewardBtn.gameObject.SetActive(open);
    }

    public void ShowWinEffect(Action onShowEnd)
    {
        AllBtnHandle(false);
        float moveX = 0 - battleWinTextImgTrm.transform.position.x;
        ShowPanel(allCvsGroup, true, () =>
        {
            winShowSequence = DOTween.Sequence()
                .Append(battleWinTextImgTrm.DOMoveX(0, 1f))
                .Append(battleWinTextImgTrm.DOMoveX(moveX, 1f).SetDelay(2.5f))
                .OnComplete(()=> { battleWinTextImgTrm.transform.position += (Vector3.left * moveX * 2); ShowPanel(rewardCvsGroup, true); onShowEnd(); });
        });
    }

    private void ShowPanel(CanvasGroup cvsGroup, bool enable, Action onShowEnd = null, bool skip = false)
    {
        showSequence.Kill();
        if (!skip)
        {
            showSequence = DOTween.Sequence().Append(cvsGroup.DOFade(enable ? 1 : 0, 0.5f).OnComplete(() => {
                cvsGroup.interactable = enable;
                cvsGroup.blocksRaycasts = enable;
                onShowEnd?.Invoke();
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
