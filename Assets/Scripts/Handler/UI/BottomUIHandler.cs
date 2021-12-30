using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomUIHandler : MonoBehaviour
{
    private int curGlod;
    public Text goldText;

    private void Start()
    {
        GameManager.Instance.OnUpdateUI += PlayGetMoneyAnim;
        GameManager.Instance.OnUpdateUI += SetGoldUI;
        SetGoldUI();
    }

    public void SetGoldUI()
    {
        goldText.text = GameManager.Instance.Gold.ToString();
        curGlod = GameManager.Instance.Gold;
    }

    public void PlayGetMoneyAnim()
    {
        int result = GameManager.Instance.Gold - curGlod;

        Anim_TextUp GetMoneyTextEffect = PoolManager.GetItem<Anim_TextUp>();
        GetMoneyTextEffect.SetType(TextUpAnimType.GetMoney);
        GetMoneyTextEffect.transform.position = goldText.transform.position;
        GetMoneyTextEffect.Play(result.ToString());
    }
}
