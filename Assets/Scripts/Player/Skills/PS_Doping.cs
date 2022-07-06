using System;

public class PS_Doping : PlayerSkill_Cooldown
{
    public int increaseValue = 2;

    public override void Cast(Action onEndSkill, Action onCancelSkill)
    {
        base.Cast(onEndSkill, onCancelSkill);

        onEndSkill += () =>
        {
            OnEndSkill();
        };

        bh.player.cc.IncreaseBuff(BuffType.Upgrade, increaseValue);

        onEndSkill?.Invoke();
    }
}
