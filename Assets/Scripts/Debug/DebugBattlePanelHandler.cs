using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugBattlePanelHandler : MonoBehaviour, IDebugPanel
{
    private BattleHandler bh;

    [Header("전투관련들")]
    public InputField playerHpField;
    public Button playerHpSubmitBtn;

    public InputField playerShieldField;
    public Button playerShieldSubmitBtn;

    public InputField enemyHpField;
    public Dropdown enemySelectDrop1;
    public Button enemyHpSubmitBtn;

    public InputField enemyShieldField;
    public Dropdown enemySelectDrop2;
    public Button enemyShieldSubmitBtn;

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
            if (!bh.isBattle)
            {
                return;
            }

            bh.BattleForceEnd();
            bh.CheckBattleEnd();
            bh.mainRullet.StopForceRullet();

            for (int i = 0; i < battleHideImgObjs.Count; i++)
            {
                battleHideImgObjs[i].SetActive(true);
            }
        });

        playerHpSubmitBtn.onClick.AddListener(OnSubmitPlayerHP);
        playerShieldSubmitBtn.onClick.AddListener(OnSubmitPlayerShield);
        enemyHpSubmitBtn.onClick.AddListener(OnSubmitEnemyHP);
        enemyShieldSubmitBtn.onClick.AddListener(OnSubmitEnemyShield);
    }

    public void OnReset()
    {
        enemySelectDrop1.options.Clear();
        enemySelectDrop2.options.Clear();

        for (int i = 0; i < battleHideImgObjs.Count; i++)
        {
            battleHideImgObjs[i].SetActive(true);
        }

        if (!bh.isBattle)
        {
            return;
        }

        for (int i = 0; i < battleHideImgObjs.Count; i++)
        {
            battleHideImgObjs[i].SetActive(false);
        }

        for (int i = 0; i < bh.enemys.Count; i++)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData
            {
                text = i.ToString(),
                image = bh.enemys[i].GetComponent<SpriteRenderer>().sprite
            };

            enemySelectDrop1.options.Add(optionData);
            enemySelectDrop2.options.Add(optionData);
        }

        Dropdown.OptionData data = new Dropdown.OptionData
        {
            text = "ALL",
            image = null
        };

        enemySelectDrop1.options.Add(data);

        enemySelectDrop1.SetValueWithoutNotify(-1);
        enemySelectDrop1.SetValueWithoutNotify(0);

        enemySelectDrop2.options.Add(data);

        enemySelectDrop2.SetValueWithoutNotify(-1);
        enemySelectDrop2.SetValueWithoutNotify(0);
    }

    private void OnSubmitPlayerHP()
    {
        bh.player.SetHp(int.Parse(playerHpField.text));
    }

    private void OnSubmitPlayerShield()
    {
        bh.player.AddShield(int.Parse(playerShieldField.text));
    }

    private void OnSubmitEnemyHP()
    {
        if (!bh.isBattle)
        {
            return;
        }

        if (enemySelectDrop1.captionText.text == "ALL")
        {
            for (int i = 0; i < bh.enemys.Count; i++)
            {
                bh.enemys[i].SetHp(int.Parse(enemyHpField.text));
            }
        }
        else
        {
            int enemyIdx = int.Parse(enemySelectDrop1.captionText.text);

            bh.enemys[enemyIdx].SetHp(int.Parse(enemyHpField.text));
        }
    }

    private void OnSubmitEnemyShield()
    {
        if (!bh.isBattle)
        {
            return;
        }

        if (enemySelectDrop2.captionText.text == "ALL")
        {
            for (int i = 0; i < bh.enemys.Count; i++)
            {
                bh.enemys[i].AddShield(int.Parse(enemyShieldField.text));
            }
        }
        else
        {
            int enemyIdx = int.Parse(enemySelectDrop2.captionText.text);

            bh.enemys[enemyIdx].AddShield(int.Parse(enemyShieldField.text));
        }
    }

    private void OnHpTextEditEnd(InputField target, string text)
    {

        if (int.TryParse(text, out int hp))
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
