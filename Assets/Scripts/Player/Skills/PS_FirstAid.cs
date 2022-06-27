using System;

public class PS_FirstAid : PlayerSkill_Cooldown
{
    public int healValue = 10;

    public override void Cast(Action onEndSkill, Action onCancelSkill)
    {
        base.Cast(onEndSkill, onCancelSkill);

        onEndSkill += () =>
        {
            ResetCooldown();
        };

        bh.player.Heal(healValue);

        onEndSkill?.Invoke();
    }
}
