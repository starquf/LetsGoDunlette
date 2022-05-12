using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillButton : MonoBehaviour
{
    [Header("Á¤º¸µé")]
    [SerializeField]
    private Image icon = null;
    [SerializeField]
    private Image coolDownImg = null;
    [SerializeField]
    private TextMeshProUGUI skillMsg = null;

    private Button skillBtn;

    public PlayerSkill currentSkill;

    private readonly string resetStr = "";

    private void Awake()
    {
        skillBtn = GetComponent<Button>();
    }

    public void Init(PlayerSkill skill, Action<PlayerSkill> onClickBtn)
    {
        currentSkill = skill;

        skill.Init(this);

        SetIcon(skill.icon);

        skillBtn.onClick.AddListener(() => 
        {
            onClickBtn?.Invoke(currentSkill);
        });

        UpdateUI();
    }

    public void SetIcon(Sprite img)
    {
        icon.sprite = img;
    }

    public void SetCoolDown(float coolDownPercent)
    {
        coolDownImg.fillAmount = coolDownPercent;
    }

    public void SetMessege(string msg)
    {
        skillMsg.text = msg;
    }

    public void UpdateUI()
    {
        ResetUI();

        currentSkill.UpdateUI(this);
    }

    private void ResetUI()
    {
        SetMessege(resetStr);
        SetCoolDown(0);
    }
}
