using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill_Cooldown : PlayerSkill
{
    [SerializeField]
    protected Image coolDownImg = null;

    protected BattleHandler bh;

    public int maxCooldown = 5;
    protected int cooldown = 0;

    [Header("»ö")]
    public Color enableColor;
    public Color disableColor;

    protected bool isFirstActivate = true;

    protected virtual void Awake()
    {
        cooldown = maxCooldown;

        isFirstActivate = true;
    }

    protected virtual void Start()
    {
        bh = GameManager.Instance.battleHandler;
    }

    public override void Init(PlayerSkillButton ui)
    {
        cooldown = maxCooldown;

        base.Init(ui);
    }

    public override void UpdateUI(PlayerSkillButton skillBtn)
    {
        SetCoolDown(cooldown / (float)maxCooldown);

        if (CanUseSkill())
        {
            skillBtn.SetStrokeColor(enableColor);

            if (isFirstActivate)
            {
                skillBtn.ShowHighlight();

                isFirstActivate = false;
            }
        }
        else
        {
            skillBtn.SetStrokeColor(disableColor);
        }
    }

    public override void Cast(Action onEndSkill)
    {
        base.Cast(onEndSkill);

        cooldown = maxCooldown;
        isFirstActivate = true;

        UpdateUI(ui);
    }

    protected virtual void SetCoolDown(float coolDownPercent)
    {
        coolDownImg.fillAmount = coolDownPercent;
    }

    public override bool CanUseSkill()
    {
        return cooldown <= 0;
    }

    public override void OnBattleStart()
    {
        bh.battleEvent.BookEvent(new NormalEvent(action =>
        {
            if (cooldown > 0)
            {
                cooldown--;
            }

            canUse = cooldown == 0;

            UpdateUI(ui);

            action?.Invoke();
        }, EventTime.StartTurn));

        /*
        bh.battleEvent.BookEvent(new NormalEvent(action =>
        {
            

            action?.Invoke();
        }, EventTime.EndBattle));
        */

        UpdateUI(ui);
    }
}
