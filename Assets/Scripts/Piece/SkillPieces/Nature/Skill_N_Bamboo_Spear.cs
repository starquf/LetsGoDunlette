using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_Bamboo_Spear : SkillPiece
{
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        return desInfos;
    }


    public override void Cast(LivingEntity target, Action onCastEnd = null) 
    {
        SetIndicator(Owner.gameObject, "Á×Ã¢").OnEndAction(() =>
        {
            target.GetDamage(GetDamageCalc(Value));
            animHandler.GetAnim(AnimName.SkillEffect01)
            .SetPosition(Owner.transform.position)
            .SetScale(2f)
            .SetRotation(Vector3.forward * -90f)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }
}
