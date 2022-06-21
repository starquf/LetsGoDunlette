using System;
using System.Collections.Generic;

public class Skill_C_Shield_Throwing : SkillPiece
{
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        LivingEntity health = Owner.GetComponent<LivingEntity>();
        if (health.HasShield())
        {
            target.GetDamage(health.GetShieldHp());
            health.RemoveShield();
        }

        animHandler.GetAnim(AnimName.E_ManaSphereHit)
            .SetScale(0.5f)
            .SetPosition(skillIconImg.transform.position)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
    }
}
