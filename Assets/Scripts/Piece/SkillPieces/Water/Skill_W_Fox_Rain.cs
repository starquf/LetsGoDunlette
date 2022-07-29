using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_W_Fox_Rain : SkillPiece
{
    public override void Cast(LivingEntity target, Action onCastEnd = null) //룰렛에 적 조각이 4개 이상일 경우 룰렛 초기화
    {
        target.cc.SetCC(CCType.Exhausted, Value);

        //if(룰렛에 적 조각이 4개 이상)
        //룰렛 초기화
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Exhausted, $"{Value}");
        return desInfos;
    }
}
