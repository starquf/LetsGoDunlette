using System;
using UnityEngine;

public class PS_Stubborn : PlayerSkill
{
    private BattleHandler bh;

    public int offensivePowerUpValue = 10;

    public int maxStack = 5;
    private int curStackCount = 0;

    private void Start()
    {
        bh = GameManager.Instance.battleHandler;

        curStackCount = 0;
    }

    public override void Cast(Action onEndSkill)
    {
        //bh.mainRullet.PauseRullet();

        //StartCoroutine(bh.battleUtil.ResetRullet(() =>
        //{
        //    StartCoroutine(bh.CheckPanelty(onEndSkill));
        //}));

        //cooldown = maxCooldown;

        //ui.UpdateUI();
    }

    public override void UpdateUI(PlayerSkillButton skillBtn)
    {
        skillBtn.SetMessege(curStackCount.ToString());
    }

    public override bool CanUseSkill()
    {
        return true;
    }

    public override void OnBattleStart()
    {
        ElementalType prevSkillType = ElementalType.None;
        curStackCount = 0;

        bh.battleEvent.BookEvent(new SkillEvent(EventTimeSkill.WithSkill, (skill, action) =>
        {
            if (!prevSkillType.Equals(skill.patternType))
            {
                //Debug.Log("고집 스택 초기화");
                //Debug.Log(prevSkillType);
                //Debug.Log(skill.patternType);
                prevSkillType = skill.patternType;
                int prevStackCount = curStackCount;
                curStackCount = 0;
                SetPlayerOffensivePower(prevStackCount);
            }

            ui.UpdateUI();

            action?.Invoke();
        }));

        bh.battleEvent.BookEvent(new SkillEvent(EventTimeSkill.AfterSkill, (skill, action) =>
        {
            if (!prevSkillType.Equals(ElementalType.Monster) && prevSkillType.Equals(skill.patternType))
            {
                //Debug.Log("고집 스택 쌓임");
                int prevStackCount = curStackCount;
                curStackCount = Mathf.Clamp(curStackCount + 1, 0, maxStack);
                SetPlayerOffensivePower(prevStackCount);
            }

            ui.UpdateUI();

            action?.Invoke();
        }));

        ui.UpdateUI();
    }

    private void SetPlayerOffensivePower(int prevStackCount)
    {
        float prevPowerUp = prevStackCount * offensivePowerUpValue;
        float powerUp = curStackCount * offensivePowerUpValue;
        bh.player.AddtionAttackPower += (int)(powerUp - prevPowerUp);
    }
}
