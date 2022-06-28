using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill_Cooldown : PlayerSkill
{
    protected Image coolDownImg = null;
    protected TextMeshProUGUI coolDownText = null;

    protected BattleHandler bh;

    public int maxCooldown = 5;
    protected int cooldown = 0;

    [Header("색")]
    public Color enableColor;
    public Color disableColor;

    protected bool isFirstActivate = true;

    [Header("리미트 설정")]
    public bool hasLimit = false;
    public int maxLimit = 4;
    protected int limit = 0;

    protected override void Awake()
    {
        base.Awake();

        coolDownImg = transform.Find("CooldownImg").GetComponent<Image>();
        coolDownText = transform.Find("CooldownText").GetComponent<TextMeshProUGUI>();

        cooldown = 0;

        isFirstActivate = true;
    }

    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;
    }

    public override void Init(PlayerSkillButton ui)
    {
        if (hasLimit)
        {
            ui.useCountUI.gameObject.SetActive(true);

            limit = maxLimit;
            ui.useCountUI.Init(maxLimit);

            ui.useCountUI.SetUseCount(limit);
        }

        cooldown = 0;

        base.Init(ui);
    }

    public override void UpdateUI(PlayerSkillButton skillBtn)
    {
        SetCoolDown(cooldown / (float)maxCooldown);

        if (cooldown <= 0)
        {
            icon.color = enableColor;
            skillBtn.SetStrokeColor(enableColor);
            coolDownText.gameObject.SetActive(false);

            if (isFirstActivate)
            {
                skillBtn.ShowHighlight();

                isFirstActivate = false;
            }
        }
        else
        {
            coolDownText.gameObject.SetActive(true);

            coolDownText.text = cooldown.ToString();

            DOTween.Sequence()
                .Append(coolDownText.transform.DOScale(Vector3.one * 1.45f, 0.15f))
                .Append(coolDownText.transform.DOScale(Vector3.one, 0.15f));

            icon.color = disableColor;
            skillBtn.SetStrokeColor(disableColor);
        }
    }

    public override void Cast(Action onEndSkill, Action onCancelSkill)
    {
        base.Cast(onEndSkill, onCancelSkill);
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

    public virtual void OnEndSkill()
    {
        if (hasLimit)
        {
            limit -= 1;

            if (limit >= 0)
            {
                // 스킬 없엠
            }

            ui.useCountUI.SetUseCount(limit);
        }

        cooldown = maxCooldown;
        isFirstActivate = true;

        UpdateUI(ui);
    }
}
