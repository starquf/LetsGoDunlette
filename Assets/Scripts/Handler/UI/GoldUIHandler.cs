using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GoldUIHandler : MonoBehaviour
{
    private Text goldText = null;
    private int prevGold = 0;

    private void Awake()
    {
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
        GameManager.Instance.battleHandler.GetComponent<BattleScrollHandler>().ShowScrollUI();

        yield return new WaitForSeconds(0.5f);

        int curGold = GameManager.Instance.Gold;
        Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
        textEffect.SetType(TextUpAnimType.GetMoney);
        textEffect.transform.position = goldText.transform.position;
        textEffect.Play((curGold - prevGold).ToString());

        goldText.text = curGold.ToString();
        prevGold = curGold;

        yield return new WaitForSeconds(0.3f);

        GameManager.Instance.battleHandler.GetComponent<BattleScrollHandler>().ShowScrollUI(open:false);
    }
}
