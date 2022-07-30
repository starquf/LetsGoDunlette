using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_F_Ifrt : SkillPiece
{
    public override void Cast(LivingEntity target, Action onCastEnd = null) //이번 전투 동안 이 조각을 포함한 현재 룰렛에 있는 모든 조각을 '재' 조각으로 변경 시킨다.
    {
        target.GetDamage(GetDamageCalc(Value));
        onCastEnd?.Invoke();
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(Value)}");
        return desInfos;
    }
}
