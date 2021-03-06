using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_F_Ashes : SkillPiece
{
    public override void Cast(LivingEntity target, Action onCastEnd = null) //이번 전투 동안 이 조각을 포함한 현재 룰렛에 있는 모든 조각을 '재' 조각으로 변경 시킨다.
    {
        target.GetDamage(Value, currentType);
        animHandler.GetAnim(AnimName.SkillEffect06)
                .SetPosition(target.transform.position)
                .SetScale(1.2f)
                .Play(() =>
                {
                    onCastEnd?.Invoke();
                });
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{Value}");
        return desInfos;
    }
}
