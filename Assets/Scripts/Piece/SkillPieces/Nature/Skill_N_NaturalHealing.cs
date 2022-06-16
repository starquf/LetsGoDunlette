using System;
using System.Collections.Generic;

public class Skill_N_NaturalHealing : SkillPiece
{
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Heal, $"{Value}");
        return desInfos;
    }
    public override void Cast(LivingEntity target, Action onCastEnd = null) //체력을 40 회복한다.
    {
        LivingEntity livingEntity = Owner.GetComponent<LivingEntity>();
        if(livingEntity.GetHpRatio() <= 0.5f)
        {
            livingEntity.Heal(5);
        }
        livingEntity.Heal(Value);

        animHandler.GetAnim(AnimName.M_Recover).SetPosition(Owner.transform.position)
            .SetScale(1)
            .Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }
}
