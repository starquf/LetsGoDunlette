using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill_Cooldown : PlayerSkill
{
    [SerializeField]
    protected Image coolDownImg = null;
    [SerializeField]
    private TextMeshProUGUI skillCooldownMsg = null;

    protected BattleHandler bh;

    public int maxCooldown = 5;
    protected int cooldown = 0;

    [Header("색")]
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
        base.Init(ui);
    }

    public override void UpdateUI(PlayerSkillButton skillBtn)
    {
        SetCoolDown(cooldown / (float)maxCooldown);

        if (CanUseSkill())
        {
            SetMessege("사용 가능");

            skillBtn.SetStrokeColor(enableColor);

            if (isFirstActivate)
            {
                skillBtn.ShowHighlight();

                isFirstActivate = false;
            }
        }
        else
        {
            SetMessege($"{cooldown}턴 남음");

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

    public virtual void SetMessege(string msg)
    {
        skillCooldownMsg.text = msg;
    }

    public override bool CanUseSkill()
    {
        return cooldown <= 0;
    }

    public override void OnBattleStart()
    {
        cooldown = maxCooldown;

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

        UpdateUI(ui);
    }
}
