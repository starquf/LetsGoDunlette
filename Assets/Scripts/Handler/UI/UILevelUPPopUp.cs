using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelUPPopUp : MonoBehaviour
{
    // 문제점
    // 2번 이상 레벨업 할때 리워드 창이 겹치게됨.
    // MaxHP 가 고정일때만 사용가능함
    // 리워드 보상이 고정임. 리워드 보상을 엑셀로 가져와서 저장해둘예정임

    public struct RewardInfo
    {
        public int atkPower;
        public int hp;
        public int maxPiece;
    }

    [Header("LevelUP")]
    public GameObject levelUPPanel;
    public Image expFillImage;
    public Image AdditiveExpFillImage;
    public Text levelText;
    public Text expText;
    public Button closeBtn;
    [Header("Reward")]
    public GameObject rewardPanel;
    public List<Button> rewardBtns;
    public List<RewardInfo> rewardInfos;

    private CanvasGroup canvasGroup;
    private PlayerHealth player;

    private void Awake()
    {
        GameManager.Instance.uILevelUPPopUp = this;
    }

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        closeBtn.onClick.AddListener(Close);

        player = GameManager.Instance.GetPlayer();


    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        closeBtn.gameObject.SetActive(false);
        rewardPanel.gameObject.SetActive(false);
        levelUPPanel.gameObject.SetActive(false);
    }

    public void OpenReward()
    {
        canvasGroup.blocksRaycasts = true;
        rewardPanel.gameObject.SetActive(true);

        for (int i = 0; i < rewardBtns.Count; i++)
        {
            rewardBtns[i].onClick.RemoveAllListeners();
        }

        for (int i = 0; i < rewardBtns.Count; i++)
        { 
            rewardBtns[i].onClick.AddListener(Close);
        }

        rewardBtns[0].onClick.AddListener(() => player.UpgradeAttackPower(5));
        rewardBtns[1].onClick.AddListener(() => player.UpgradeHP(10));
        rewardBtns[2].onClick.AddListener(() => player.UpgradeMaxPieceCount(1));
    }

    public void PopUp(int originLevel, int originExp, int nowLevel, int nowExp, int maxExp, bool isFirst = true)
    {
        canvasGroup.blocksRaycasts = true;
        closeBtn.gameObject.SetActive(false);
        levelUPPanel.gameObject.SetActive(true);
        StartCoroutine(StartPopUp(originLevel, originExp, nowLevel, nowExp, maxExp, isFirst));
    }

    public IEnumerator StartPopUp(int originLevel, int originExp, int nowLevel, int nowExp, int maxExp, bool isFirst = true) //exp가 고정이 아니게 되면 변경해야함
    {
        float time = 0.7f;
        expFillImage.color = Color.white;
        expText.color = Color.white;
        levelText.text = $"{originLevel}";
        expText.text = $"{originExp}/{maxExp}";

        float tempRatio = originExp / (float)maxExp;

        expFillImage.fillAmount = 0;
        AdditiveExpFillImage.fillAmount = 0;
        if (isFirst)
        {
            canvasGroup.DOFade(1f, time);
            yield return new WaitForSeconds(time);
            DOTween.To(() => expFillImage.fillAmount, x => expFillImage.fillAmount = x, tempRatio, time);
            yield return new WaitForSeconds(time);
        }

        if (nowLevel > originLevel)
        {
            DOTween.To(() => AdditiveExpFillImage.fillAmount, x => AdditiveExpFillImage.fillAmount = x, 1, time);
            yield return new WaitForSeconds(time);
            DOTween.To(() => expFillImage.fillAmount, x => expFillImage.fillAmount = x, 1, time);
            expText.DOText($"{maxExp}/{maxExp}", time);
            yield return new WaitForSeconds(time);
            expFillImage.DOColor(new Color(0, 0.6787322f, 1), time / 2);
            expText.DOColor(new Color(0, 0.6787322f, 1), time / 2);
            yield return new WaitForSeconds(time);
            OpenReward(); 
            PopUp(originLevel + 1, 0, nowLevel, nowExp, maxExp, false);
        }
        else
        {
            tempRatio = nowExp / (float)maxExp;

            DOTween.To(() => AdditiveExpFillImage.fillAmount, x => AdditiveExpFillImage.fillAmount = x, tempRatio, time);
            yield return new WaitForSeconds(time);
            DOTween.To(() => expFillImage.fillAmount, x => expFillImage.fillAmount = x, tempRatio, time);
            expText.DOText($"{nowExp}/{maxExp}", time);
            closeBtn.gameObject.SetActive(true);
        }

        yield return null;
    }
}
