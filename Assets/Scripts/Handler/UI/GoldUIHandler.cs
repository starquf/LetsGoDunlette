using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GoldUIHandler : MonoBehaviour
{
    private RectTransform thisRectTrm;
    private Text goldText = null;
    private int prevGold = 0;

    private Sequence goldUISequence;

    private BattleHandler bh;

    private void Awake()
    {
        GameManager.Instance.goldUIHandler = this;

        thisRectTrm = GetComponent<RectTransform>();
        goldText = GetComponentInChildren<Text>();

    }

    private void Start()
    {
        Init();
        prevGold = GameManager.Instance.Gold;
        bh = GameManager.Instance.battleHandler;
        GameManager.Instance.OnUpdateUI += UpdateGoldUI;
    }

    private void Init()
    {
        int curGold = GameManager.Instance.Gold;
        goldText.text = curGold.ToString();
        prevGold = curGold;
    }
    //public void ShowGoldUI(bool open = true, bool skip = false)
    //{
    //    if (skip)
    //    {
    //        thisRectTrm.anchoredPosition = new Vector2(open ? 0f : -230f, thisRectTrm.anchoredPosition.y);
    //        ShowGoldText(open, true);
    //    }
    //    else
    //    {
    //        goldUISequence.Kill();

    //        goldUISequence = DOTween.Sequence()
    //        .Append(thisRectTrm.DOAnchorPosX(open ? 0f : -230f, 0.5f))
    //        .OnComplete(() =>
    //        {
    //            ShowGoldText(open);
    //        });
    //    }
    //}

    public void ShowGoldText(bool open = true, bool skip = false)
    {
        goldText.DOFade(open ? 1 : 0, skip ? 0f : 0.3f);
    }

    private void UpdateGoldUI()
    {
        GetMoneyAnim();
        //StartCoroutine(UpdateGoldUIAnim());
    }


    //private IEnumerator UpdateGoldUIAnim()
    //{
    //    if (bh.isBattle)
    //    {
    //        GetMoneyAnim();
    //    }
    //    else
    //    {

    //        if (GameManager.Instance.curEncounter != mapNode.SHOP)
    //        {
    //            ShowGoldUI();
    //        }
    //        yield return new WaitForSeconds(0.5f);

    //        GetMoneyAnim();

    //        yield return new WaitForSeconds(0.5f);

    //        if (GameManager.Instance.curEncounter != mapNode.SHOP)
    //        {
    //            ShowGoldUI(false);
    //        }
    //    }
    //}

    private void GetMoneyAnim()
    {
        int curGold = GameManager.Instance.Gold;
        GameManager.Instance.animHandler.GetTextAnim()
        .SetType(TextUpAnimType.Up)
        .SetPosition(goldText.transform.position)
        .Play((curGold - prevGold).ToString());

        goldText.text = curGold.ToString();
        prevGold = curGold;
    }
}
