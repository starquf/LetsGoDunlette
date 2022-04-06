using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugBattlePanelHandler : MonoBehaviour, IDebugPanel
{
    private BattleHandler bh;

    [Header("전투관련들")]
    public InputField playerHpField;
    public InputField enemyHpField;
    public Dropdown enemySelectDrop;
    public Button hpSubmitBtn;
    public Button finishBattleBtn;

    [Space(10f)]
    public List<GameObject> battleHideImgObjs = new List<GameObject>();

    public void Init()
    {
        bh = GameManager.Instance.battleHandler;

        playerHpField.onEndEdit.AddListener(text =>
        {
            OnHpTextEditEnd(playerHpField, text);
        });

        enemyHpField.onEndEdit.AddListener(text =>
        {
            OnHpTextEditEnd(enemyHpField, text);
        });

        finishBattleBtn.onClick.AddListener(() =>
        {
            if (!bh.isBattle) return;

            bh.BattleForceEnd();
            bh.CheckBattleEnd();
            bh.mainRullet.StopForceRullet();
            GameManager.Instance.inventoryHandler.RemoveNull();

            for (int i = 0; i < battleHideImgObjs.Count; i++)
            {
                battleHideImgObjs[i].SetActive(true);
            }
        });

        hpSubmitBtn.onClick.AddListener(OnSubmitHp);
    }

    public void OnReset()
    {
        enemySelectDrop.options.Clear();

        for (int i = 0; i < battleHideImgObjs.Count; i++)
        {
            battleHideImgObjs[i].SetActive(true);
        }

        if (!bh.isBattle) return;

        for (int i = 0; i < battleHideImgObjs.Count; i++)
        {
            battleHideImgObjs[i].SetActive(false);
        }

        for (int i = 0; i < bh.enemys.Count; i++)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = i.ToString();
            optionData.image = bh.enemys[i].GetComponent<SpriteRenderer>().sprite;

            enemySelectDrop.options.Add(optionData);
        }

        Dropdown.OptionData data = new Dropdown.OptionData();
        data.text = "ALL";
        data.image = null;

        enemySelectDrop.options.Add(data);

        enemySelectDrop.SetValueWithoutNotify(-1);
        enemySelectDrop.SetValueWithoutNotify(0);
    }

    private void OnSubmitHp()
    {
        bh.player.SetHp(int.Parse(playerHpField.text));

        if (!bh.isBattle)
        {
            return;
        }

        if (enemySelectDrop.captionText.text == "ALL")
        {
            for (int i = 0; i < bh.enemys.Count; i++)
            {
                bh.enemys[i].SetHp(int.Parse(enemyHpField.text));
            }
        }
        else
        {
            int enemyIdx = int.Parse(enemySelectDrop.captionText.text);

            bh.enemys[enemyIdx].SetHp(int.Parse(enemyHpField.text));
        }
    }

    private void OnHpTextEditEnd(InputField target, string text)
    {
        int hp = 0;

        if (int.TryParse(text, out hp))
        {
            if (hp <= 0)
            {
                target.text = "1";
            }
        }
        else
        {
            target.text = "1";
        }
    }
}
