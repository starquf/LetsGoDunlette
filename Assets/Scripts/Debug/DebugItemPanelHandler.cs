using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugItemPanelHandler : MonoBehaviour, IDebugPanel
{
    private BattleHandler bh;

    [Header("아이템 관련들")]
    public Dropdown scrollDropdown;
    public Button scrollGetBtn;

    [Space(10f)]
    public InputField goldInput;
    public Button goldGetBtn;
    public Button goldRemoveBtn;

    public void Init()
    {
        bh = GameManager.Instance.battleHandler;

        InitScrollOptions();

        scrollGetBtn.onClick.AddListener(GetScroll);

        goldInput.onEndEdit.AddListener(text =>
        {
            OnGoldEditEnd(text);
        });

        goldGetBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.Gold += int.Parse(goldInput.text);
        });

        goldRemoveBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.Gold -= int.Parse(goldInput.text);

            if (GameManager.Instance.Gold <= 0)
            {
                GameManager.Instance.Gold = 0;
            }
        });
    }

    private void OnGoldEditEnd(string text)
    {
        int gold = 0;

        if (int.TryParse(text, out gold))
        {
            if (gold <= 0)
            {
                goldInput.text = "1";
            }
        }
        else
        {
            goldInput.text = "1";
        }
    }

    private void InitScrollOptions()
    {
        scrollDropdown.options.Clear();

        foreach (ScrollType scroll in Enum.GetValues(typeof(ScrollType)))
        {
            Scroll scrollObj = PoolManager.GetScroll(scroll);

            Dropdown.OptionData op = new Dropdown.OptionData();
            op.text = scroll.ToString();
            op.image = scrollObj.GetComponent<Image>().sprite;

            scrollDropdown.options.Add(op);

            scrollObj.gameObject.SetActive(false);
        }

        scrollDropdown.SetValueWithoutNotify(-1);
        scrollDropdown.SetValueWithoutNotify(0);
    }

    private void GetScroll()
    {
        BattleScrollHandler sh = bh.battleScroll;

        if (sh.IsFullScroll())
        {
            sh.slots[0].RemoveScroll();
            sh.SortScroll();
        }

        ScrollType scroll = (ScrollType)Enum.Parse(typeof(ScrollType), scrollDropdown.captionText.text);
        sh.GetScroll(PoolManager.GetScroll(scroll));
    }

    public void OnReset()
    {

    }
}
