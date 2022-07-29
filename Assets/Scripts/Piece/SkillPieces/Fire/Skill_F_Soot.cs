using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_F_Soot : SkillPiece
{
    public override void Cast(LivingEntity target, Action onCastEnd = null) 
    {
        target.GetDamage(GetDamageCalc(value));
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{Value}");
        return desInfos;
    }
}
