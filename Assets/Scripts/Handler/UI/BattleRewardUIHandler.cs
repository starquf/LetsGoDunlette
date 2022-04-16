using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleRewardUIHandler : MonoBehaviour
{
    private CanvasGroup cg;

    [Header("캔버스 그룹들")]
    [SerializeField] private CanvasGroup rewardCG;
    public CanvasGroup selectCG;
    public CanvasGroup pieceDesCG;
    public CanvasGroup buttonCG;

    [Header("버튼들")]
    public Button getBtn;
    public Button skipBtn;

    [Header("그외")]
    [SerializeField]
    private Transform battleWinImgTrans;
    [SerializeField]
    private Transform selectContext;
    [SerializeField]
    private Text rewardText;

    [SerializeField] private List<ParticleSystem> fireworks = new List<ParticleSystem>();

    [Header("카드 UI")]
    public Image cardBG;
    public Text cardNameText;
    public Text cardDesText;
    public Image bookmarkBG;
    public Image bookmarkIcon;
    public Image maskImg;
    public Image selectedImg;

    private Sequence showSequence;
    private Sequence winShowSequence;
    private Tween rewardTextTween;

    private Vector2 startPos;

    private InventoryHandler invenHandler;
    private BattleHandler bh;

    [HideInInspector]
    public SkillPiece selectedSkillObj;

    private WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        invenHandler = GameManager.Instance.inventoryHandler;
        bh = GameManager.Instance.battleHandler;

        startPos = pieceDesCG.transform.localPosition;
        ResetRewardUI();
    }

    public void ResetRewardUI()
    {
        ShowPanel(cg, false, skip: true);
        ShowPanel(rewardCG, false, skip: true);
        ShowPanel(selectCG, false, skip: true);
        ShowPanel(pieceDesCG, false, skip: true);
        ShowPanel(buttonCG, false, skip: true);

        rewardTextTween.Kill();
        rewardText.color = new Color(1f, 1f, 1f, 0f);
        selectedImg.gameObject.SetActive(false);

        rewardText.transform.localScale = Vector3.one;

        getBtn.gameObject.SetActive(false);

        pieceDesCG.transform.localPosition = startPos;
        pieceDesCG.transform.eulerAngles = Vector3.zero;
        pieceDesCG.transform.localScale = Vector3.one;

        cardBG.color = Color.white;
        cardNameText.color = Color.white;
        cardDesText.color = Color.white;
        bookmarkBG.color = Color.white;
        bookmarkIcon.color = Color.white;

        //maskImg.color = Color.white;
        print(cardBG.material.GetFloat("_DesolveIntensity"));

        cardBG.material.SetFloat("_DesolveIntensity", -1f);
    }

    public void ShowWinEffect(Action onShowEnd = null)
    {
        //float moveX = 0 - battleWinTextImgTrm.transform.position.x;

        Image battleWinImg = battleWinImgTrans.GetComponent<Image>();
        battleWinImg.color = new Color(1f, 1f, 1f, 0f);

        battleWinImgTrans.localPosition = new Vector3(0f, 200f);

        ShowPanel(cg, true, () =>
        {
            winShowSequence = DOTween.Sequence()
                .AppendInterval(0.15f)
                .Append(battleWinImg.DOFade(1f, 0.8f))
                .InsertCallback(0.5f, () => {
                    for (int i = 0; i < fireworks.Count; i++)
                    {
                        fireworks[i].Play();
                    }
                })
                .AppendInterval(0.6f)
                .Append(battleWinImgTrans.DOLocalMoveY(1150f, 0.8f))
                .Join(battleWinImg.DOFade(0f, 0.5f))
                .AppendCallback(() => 
                {
                    ShowPanel(rewardCG, true);
                })
                .Append(rewardCG.transform.DOLocalMoveY(1000f, 0.9f)
                        .From()
                        .SetEase(Ease.OutBounce))
                .AppendCallback(() =>
                {
                    onShowEnd?.Invoke();
                });
        });
    }

    public void ShowReward(List<SkillPiece> rewards)
    {
        StartCoroutine(CreateReward(rewards));

        Sequence seq = DOTween.Sequence()
            .Append(rewardText.DOFade(1f, 1f).From(0f).SetEase(Ease.Linear))
            .InsertCallback(0.16f, () =>
            {
                ShowPanel(selectCG, true, dur: 0.8f);

                rewardTextTween = rewardText.transform.DOScale(1.07f, 0.8f)
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetEase(Ease.Linear);
            })
            .InsertCallback(0.46f, () =>
             {
                 ShowPanel(buttonCG, true, dur: 0.8f);
             });
        //.Append(selectCG.transform.DOScaleX(1f, 0.25f).From(0.55f));

    }

    private IEnumerator CreateReward(List<SkillPiece> rewards)
    {
        List<PieceInfoUI> infoUIs = new List<PieceInfoUI>();
        selectContext.GetComponentsInChildren(infoUIs);

        for (int i = 0; i < infoUIs.Count; i++)
        {
            infoUIs[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < rewards.Count; i++)
        {
            SkillPiece reward = rewards[i];

            PieceInfoUI pieceInfoUI = PoolManager.GetItem<PieceInfoUI>();
            pieceInfoUI.SetSkillIcon(reward.transform.Find("SkillIcon").GetComponent<Image>().sprite);


            pieceInfoUI.GetComponent<Image>().DOFade(1f, 0.5f)
                .From(0f)
                .SetEase(Ease.Linear);


            pieceInfoUI.transform.SetParent(selectContext);

            pieceInfoUI.button.onClick.RemoveAllListeners();
            pieceInfoUI.button.onClick.AddListener(() =>
            {
                selectedSkillObj = reward;

                selectedImg.gameObject.SetActive(true);
                selectedImg.transform.position = pieceInfoUI.transform.position;

                ShowDesPanel(reward);
            });

            //pieceInfoUI.transform.SetAsFirstSibling();

            yield return pOneSecWait;
        }
    }

    public void ShowDesPanel(SkillPiece info)
    {
        cardBG.sprite = info.cardBG;
        cardNameText.text = info.PieceName;
        cardDesText.text = info.PieceDes;
        bookmarkBG.sprite = invenHandler.bookmarkSprDic[info.patternType];
        bookmarkIcon.sprite = invenHandler.effectSprDic[info.patternType];

        ShowPanel(pieceDesCG, true);
        getBtn.gameObject.SetActive(true);
    }

    public void GetRewardEffect(Action onEndEffect = null)
    {
        Anim_RewardDetermined rewardEffect = PoolManager.GetItem<Anim_RewardDetermined>();
        rewardEffect.SetScale(1.5f);
        //rewardEffect.ChangeColor(bh.castUIHandler.colorDic[selectedSkillObj.patternType]);
        rewardEffect.transform.position = cardBG.transform.position;

        rewardEffect.Play();

        Sequence seq = DOTween.Sequence()
            .AppendInterval(0.25f)
            .Append(DOTween.To(() => cardBG.material.GetFloat("_DesolveIntensity"),
                        x => cardBG.material.SetFloat("_DesolveIntensity", x)
                        , 1f
                        , 1f))
            .Join(cardNameText.DOFade(0f, 0.9f))
            .Join(cardDesText.DOFade(0f, 0.9f))
            .Join(bookmarkBG.DOFade(0f, 0.9f))
            .Join(bookmarkIcon.DOFade(0f, 0.9f))
            .AppendCallback(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    int a = i;

                    EffectObj effect = PoolManager.GetItem<EffectObj>();
                    effect.SetSprite(invenHandler.effectSprDic[selectedSkillObj.patternType]);
                    effect.SetColorGradient(invenHandler.effectGradDic[selectedSkillObj.patternType]);

                    effect.transform.position = pieceDesCG.transform.position;

                    effect.transform.DOMove(Random.insideUnitCircle * 1.5f, 0.4f)
                        .SetRelative();

                    effect.Play(invenHandler.transform.position, () =>
                    {
                        if (a == 9)
                            onEndEffect?.Invoke();

                        effect.EndEffect();
                    }
                    , BezierType.Quadratic, delay: 0.4f);
                }

                ShowPanel(pieceDesCG, false, skip: true);
            });
    }

    public void SkipRewardEffect(Action onEndEffect = null)
    {
        pieceDesCG.transform.DORotate(new Vector3(0f, 0f, 90f), 0.5f);
        pieceDesCG.transform.DOLocalMoveY(800f, 0.5f)
            .SetRelative()
            .OnComplete(() => 
            {
                onEndEffect?.Invoke();
            });
    }

    private void ShowPanel(CanvasGroup cvsGroup, bool enable, Action onShowEnd = null, bool skip = false, float dur = 0.5f)
    {
        if (!skip)
        {
             showSequence = DOTween.Sequence()
                .Append(cvsGroup.DOFade(enable ? 1 : 0, dur)
                .OnComplete(() => {
                    onShowEnd?.Invoke();
                }));

            cvsGroup.interactable = enable;
            cvsGroup.blocksRaycasts = enable;
        }
        else
        {
            cvsGroup.alpha = enable ? 1 : 0;
            cvsGroup.interactable = enable;
            cvsGroup.blocksRaycasts = enable;

            onShowEnd?.Invoke();
        }
    }
}
