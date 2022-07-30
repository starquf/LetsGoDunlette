using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_E_Rail_Gun : SkillPiece
{
    public override void Cast(LivingEntity target, Action onCastEnd = null) //사용 시 <sprite=11>를 4턴 받는다.
    {
        target.GetDamage(GetDamageCalc(Value));
        Owner.GetComponent<LivingEntity>().cc.SetCC(CCType.Exhausted, 5);
        onCastEnd?.Invoke();
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(Value)}");
        return desInfos;
    }
}
