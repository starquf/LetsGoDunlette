using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_C_T_Surprisebox : SkillPiece
{
    protected override void Start()
    {
        base.Start();
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Stun, $"{Value}%");
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) //±âÀý	100%
    {
        if (UnityEngine.Random.Range(0, 100) <= Value)
        {
            target.cc.SetCC(CCType.Stun, 1);
        }
        onCastEnd?.Invoke();
    }
}
