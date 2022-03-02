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
    [SerializeField] private CanvasGroup selectCardCvsGroup;
    [SerializeField] private Button rewardBtn;
    [SerializeField] private Button selectCancelBtn;
    [SerializeField] private Button selectBtn;
    [SerializeField] private Button rewardSkipBtn;
    [SerializeField] private Text rewardBtnText;
    [SerializeField] private Text rewardSkipBtnText;

    [Header("카드 UI")]
    public Image cardBG;
    public Text cardNameText;
    public Text cardDesText;

    private Sequence showSequence;
    private Sequence winShowSequence;

    private void Awake()
    {
        allCvsGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        rewardSkipBtnText.text = "넘기기";
        rewardBtnText.text = "확인";
        selectBtn.onClick.AddListener(() =>
        {
            AllBtnHandle(false);
            ShowPanel(selectCardCvsGroup, true);
        });
        selectCancelBtn.onClick.AddListener(() =>
        {
            rewardBtn.interactable = false;
            selectCancelBtn.interactable = false;
            ShowPanel(selectCardCvsGroup, false, ()=> {
                AllBtnHandle(true);
                rewardBtn.interactable = true;
                selectCancelBtn.interactable = true;
            });
        });
    }

    public void SetButton(SkillPiece skillPiece, Action onClickGet, Action onClickSkip)
    {
        cardBG.sprite = skillPiece.cardBG;
        cardNameText.text = skillPiece.PieceName;
        cardDesText.text = skillPiece.PieceDes;

        rewardBtn.onClick.RemoveAllListeners();
        rewardSkipBtn.onClick.RemoveAllListeners();

        rewardBtn.onClick.AddListener(() => {
            rewardBtn.interactable = false;
            selectCancelBtn.interactable = false;
            onClickGet();
            ShowPanel(allCvsGroup, false, ()=> {
                ShowPanel(rewardCvsGroup, false, null, true);
                ShowPanel(selectCardCvsGroup, false, null, true);
                rewardBtn.interactable = true;
                selectCancelBtn.interactable = true;
                rewardBtn.gameObject.SetActive(true);
            });
            rewardBtn.gameObject.SetActive(false);
        });
        rewardSkipBtn.onClick.AddListener(() => {
            rewardSkipBtn.interactable = false;
            selectBtn.interactable = false;
            onClickSkip();
            ShowPanel(allCvsGroup, false, () => {
                ShowPanel(rewardCvsGroup, false, null, true);
                rewardSkipBtn.interactable = true;
                selectBtn.interactable = true;
            });
            AllBtnHandle(false);
        });
        AllBtnHandle(true);
        rewardSkipBtn.gameObject.SetActive(true);
    }

    public void AllBtnHandle(bool open)
    {
        rewardSkipBtn.gameObject.SetActive(open);
        selectBtn.gameObject.SetActive(open);
    }

    public void ShowWinEffect(Action onShowEnd)
    {
        AllBtnHandle(false);
        //float moveX = 0 - battleWinTextImgTrm.transform.position.x;

        Image battleWinTextImg = battleWinTextImgTrm.GetComponent<Image>();
        battleWinTextImg.color = new Color(1f, 1f, 1f, 0f);

        battleWinTextImgTrm.localPosition = new Vector3(0f, 200f);

        ShowPanel(allCvsGroup, true, () =>
        {
            winShowSequence = DOTween.Sequence()
                .AppendInterval(0.15f)
                .Append(battleWinTextImg.DOFade(1f, 0.8f))
                .AppendInterval(0.6f)
                .Append(battleWinTextImgTrm.DOLocalMoveY(1150f, 0.8f))
                .OnComplete(()=> { 
                    rewardCvsGroup.transform.DOLocalMoveY(1000f, 0.9f).From().SetEase(Ease.OutBounce);
                    ShowPanel(rewardCvsGroup, true); 
                    onShowEnd(); 
                });
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
