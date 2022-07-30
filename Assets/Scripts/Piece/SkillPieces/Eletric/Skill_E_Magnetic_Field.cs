using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_E_Magnetic_Field : SkillPiece
{
    public int exhaustedTurn;
    public override void Cast(LivingEntity target, Action onCastEnd = null) 
    {
        target.GetDamage(GetDamageCalc(Value));
        target.cc.SetCC(CCType.Exhausted, exhaustedTurn + 1);
        onCastEnd?.Invoke();
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(Value)}");
        desInfos[1].SetInfo(DesIconType.Exhausted, $"{exhaustedTurn}");
        return desInfos;
    }
}
