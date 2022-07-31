using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_F_Flashover : SkillPiece
{
    //사용 시 '가열' 조각으로 변경된다.
    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        target.GetDamage(GetDamageCalc(Value), currentType);
        onCastEnd?.Invoke();
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(Value)}");
        return desInfos;
    }
}
