using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ExpLog
{
    public string content;
    public int expValue;
    public ExpLog(string content, int expValue)
    {
        this.content = content;
        this.expValue = expValue;
    }
}

public class UILevelUPPopUp : MonoBehaviour
{
    // 문제점
    // 2번 이상 레벨업 할때 리워드 창이 겹치게됨.
    // MaxHP 가 고정일때만 사용가능함
    // 리워드 보상이 고정임. 리워드 보상을 엑셀로 가져와서 저장해둘예정임

    [Serializable]
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
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI expLogText;
    public Button closeBtn;
    [Header("Reward")]
    public GameObject rewardPanel;
    public List<Button> rewardBtns;
    public List<GameObject> rewardGOs;
    public List<TextMeshProUGUI> beforeAfterTxts;
    public List<RewardInfo> rewardInfos;

    private List<TextMeshProUGUI> expTxts = new List<TextMeshProUGUI>();
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
        expTxts.ForEach((a) => a.gameObject.SetActive(false));
        GameManager.Instance.EndEncounter();
    }

    public void OpenReward()
    {
        canvasGroup.blocksRaycasts = true;
        rewardPanel.gameObject.SetActive(true);

        for (int i = 0; i < rewardBtns.Count; i++)
        {
            rewardBtns[i].onClick.RemoveAllListeners();
        }


        beforeAfterTxts[0].text = $"{player.AttackPower} <color=yellow>▶ {player.AttackPower + rewardInfos[player.atkLevel].atkPower}</color>";
        rewardBtns[0].onClick.AddListener(() =>
        {
            player.UpgradeAttackPower(rewardInfos[player.atkLevel].atkPower);
            player.atkLevel++;
        });
        beforeAfterTxts[1].text = $"{player.maxHp} <color=yellow>▶ {player.maxHp + rewardInfos[player.hpLevel].hp}</color>";
        rewardBtns[1].onClick.AddListener(() =>
        {
            player.UpgradeHP(rewardInfos[player.hpLevel].hp);
            player.hpLevel++;
        });
        beforeAfterTxts[2].text = $"{player.MaxPieceCount} <color=yellow>▶ {player.MaxPieceCount + rewardInfos[player.maxPieceLevel].maxPiece}</color>";
        rewardBtns[2].onClick.AddListener(() =>
        {
            player.UpgradeMaxPieceCount(rewardInfos[player.maxPieceLevel].maxPiece);
            player.maxPieceLevel++;
        });

        for (int i = 0; i < rewardBtns.Count; i++)
        {
            rewardBtns[i].onClick.AddListener(Close);
            rewardBtns[i].interactable = true;
            rewardGOs[i].SetActive(true);
        }

        if (rewardInfos[player.atkLevel].atkPower == 0)
        {
            rewardBtns[0].interactable = false;
            rewardGOs[0].SetActive(false);
        }

        if (rewardInfos[player.hpLevel].hp == 0)
        {
            rewardBtns[1].interactable = false;
            rewardGOs[1].SetActive(false);
        }

        if (rewardInfos[player.maxPieceLevel].maxPiece == 0)
        {
            rewardBtns[2].interactable = false;
            rewardGOs[2].SetActive(false);
        }
    }

    public void PopUp(int originLevel, int originExp, int nowLevel, int nowExp, int maxExp, List<ExpLog> expLogs = null, bool isFirst = true)
    {
        canvasGroup.blocksRaycasts = true;
        closeBtn.gameObject.SetActive(false);
        levelUPPanel.gameObject.SetActive(true);
        StartCoroutine(StartPopUp(originLevel, originExp, nowLevel, nowExp, maxExp, expLogs, isFirst));
    }

    public IEnumerator StartPopUp(int originLevel, int originExp, int nowLevel, int nowExp, int maxExp, List<ExpLog> expLogs = null, bool isFirst = true) //exp가 고정이 아니게 되면 변경해야함
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
            StartCoroutine(ExpLog(expLogs));

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
            expText.text = $"{maxExp}/{maxExp}";
            yield return new WaitForSeconds(time);
            expFillImage.DOColor(new Color(0, 0.6787322f, 1), time / 2);
            expText.DOColor(new Color(0, 0.6787322f, 1), time / 2);
            levelText.text = $"{originLevel + 1}";
            yield return new WaitForSeconds(time * 2);
            OpenReward();
            PopUp(originLevel + 1, 0, nowLevel, nowExp, maxExp, expLogs, false);
        }
        else
        {
            tempRatio = nowExp / (float)maxExp;

            DOTween.To(() => AdditiveExpFillImage.fillAmount, x => AdditiveExpFillImage.fillAmount = x, tempRatio, time);
            yield return new WaitForSeconds(time);
            DOTween.To(() => expFillImage.fillAmount, x => expFillImage.fillAmount = x, tempRatio, time);
            expText.text = $"{nowExp}/{maxExp}";
            closeBtn.gameObject.SetActive(true);
        }

        yield return null;
    }
    public IEnumerator ExpLog(List<ExpLog> expLogs)
    {
        if (expLogs == null)
        {
            yield break;
        }

        for (int i = 0; i < expLogs.Count; i++)
        {
            string logText = $"{expLogs[i].content} +{expLogs[i].expValue}";

            if (expTxts.Count <= i)
            {
                TextMeshProUGUI textLog = Instantiate(expLogText, levelUPPanel.transform).GetComponent<TextMeshProUGUI>();
                expTxts.Add(textLog);
                textLog.text = logText;
                textLog.color = new Color(1, 1, 1, 0);
                textLog.DOColor(new Color(1, 1, 1, 1), 0.5f);

                var expRect = textLog.GetComponent<RectTransform>();
                expRect.localPosition = new Vector2(0, i * -100);

                textLog.gameObject.SetActive(true);
            }
            else
            {
                TextMeshProUGUI textLog = expTxts[i];
                textLog.text = logText;
                textLog.color = new Color(1, 1, 1, 0);
                textLog.DOColor(new Color(1, 1, 1, 1), 0.5f);

                var expRect = textLog.GetComponent<RectTransform>();
                expRect.localPosition = new Vector2(0, i * -100);

                textLog.gameObject.SetActive(true);
            }

            //expRect.DOAnchorPosY(expRect.position.y + 40, 0.5f);

            yield return new WaitForSeconds(0.6f);
        }

        yield return null;
    }
}
