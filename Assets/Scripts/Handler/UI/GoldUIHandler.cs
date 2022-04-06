using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GoldUIHandler : MonoBehaviour
{
    private RectTransform thisRectTrm;
    private Text goldText = null;
    private int prevGold = 0;

    private Sequence goldUISequence;

    private void Awake()
    {
        GameManager.Instance.goldUIHandler = this;

        thisRectTrm = GetComponent<RectTransform>();
        goldText = GetComponentInChildren<Text>();
    }
    void Start()
    {
        Init();
        prevGold = GameManager.Instance.Gold;
        GameManager.Instance.OnUpdateUI += UpdateGoldUI;
    }

    private void Init()
    {
        int curGold = GameManager.Instance.Gold;
        goldText.text = curGold.ToString();
        prevGold = curGold;
    }
    public void ShowGoldUI(bool open = true, bool skip = false)
    {
        if (skip)
        {
            thisRectTrm.anchoredPosition = new Vector2(open ? 0f : -230f, thisRectTrm.anchoredPosition.y);
            ShowGoldText(open, true);
        }
        else
        {
            goldUISequence.Kill();
            goldUISequence = DOTween.Sequence()
                .Append(thisRectTrm.DOAnchorPosX(open ? 0f : -230f, 0.5f))
                .OnComplete(() =>
                {
                    ShowGoldText(open);
                });
        }
    }

    public void ShowGoldText(bool open = true, bool skip = false)
    {
        goldText.DOFade(open ? 1 : 0, skip ? 0f : 0.3f);
    }

    private void UpdateGoldUI()
    {
        StartCoroutine(UpdateGoldUIAnim());
    }


    private IEnumerator UpdateGoldUIAnim()
    {
        BattleHandler bh = GameManager.Instance.battleHandler;
        if(bh.isBattle)
        {
            GetMoneyAnim();
        }
        else
        {
            ShowGoldUI();

            yield return new WaitForSeconds(0.5f);

            GetMoneyAnim();

            yield return new WaitForSeconds(0.5f);

            ShowGoldUI(false);
        }
    }

    private void GetMoneyAnim()
    {
        int curGold = GameManager.Instance.Gold;
        Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
        textEffect.SetType(TextUpAnimType.GetMoney);
        textEffect.transform.position = goldText.transform.position;
        textEffect.Play((curGold - prevGold).ToString());

        goldText.text = curGold.ToString();
        prevGold = curGold;
    }
}
