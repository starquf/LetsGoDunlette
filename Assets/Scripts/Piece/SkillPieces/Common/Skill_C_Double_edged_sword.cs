using System;
using System.Collections.Generic;

public class Skill_C_Double_edged_sword : SkillPiece
{
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        LivingEntity playerHealth = Owner.GetComponent<LivingEntity>();
        if (!playerHealth.HasShield())
        {
            playerHealth.GetDamage(2);
            target.GetDamage(GetDamageCalc(), currentType);
        }
        else
        {
            target.GetDamage(GetDamageCalc(), currentType);
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
