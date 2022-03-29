using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUIHandler : MonoBehaviour
{
    [Header("디버그 모드 활성화 여부")]
    public bool isDebug = true;

    [Header("디버그 UI 옵젝들")]
    public CanvasGroup debugPanel;

    [Header("메인 버튼들")]
    [Space(10f)]
    public Button openBtn;
    public Button closeBtn;
    public Button resetBtn;

    [Header("전투관련들")]
    [Space(10f)]
    public InputField playerHpField;
    public InputField enemyHpField;
    public Dropdown enemySelectDrop;
    public Button hpSubmitBtn;
    public Button finishBattleBtn;

    [Space(10f)]
    public List<GameObject> battleHideImgObjs = new List<GameObject>();

    private BattleHandler bh;

    private void Start()
    {
        if (!isDebug)
        {
            gameObject.SetActive(false);
            return;
        }

        Init();

        SetDebugPanel(false);

        bh = GameManager.Instance.battleHandler;
    }


    #region Inits
    private void Init()
    {
        MainBtnInit();
        BattleHpInit();
    }

    private void BattleHpInit()
    {
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
        });

        hpSubmitBtn.onClick.AddListener(OnSubmitHp);
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

    private void MainBtnInit()
    {
        openBtn.onClick.AddListener(() =>
        {
            Time.timeScale = 0f;

            ResetDebugPanel();
            SetDebugPanel(true);
        });

        closeBtn.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;

            SetDebugPanel(false);
        });

        resetBtn.onClick.AddListener(ResetDebugPanel);
    }
    #endregion


    #region Reset Func
    private void ResetDebugPanel()
    {
        BattleHpReset();
    }

    private void BattleHpReset()
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
    #endregion

    private void SetDebugPanel(bool enable)
    {
        debugPanel.alpha = enable ? 1f : 0f;
        debugPanel.blocksRaycasts = enable;
        debugPanel.interactable = enable;
    }
}
