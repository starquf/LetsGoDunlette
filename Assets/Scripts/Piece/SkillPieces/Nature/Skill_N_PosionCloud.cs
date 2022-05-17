using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_PosionCloud : SkillPiece
{
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[1].SetInfo(DesIconType.Wound, "2");

        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        Vector3 targetPos = target.transform.position;

        animHandler.GetAnim(AnimName.N_PoisionCloud)
                .SetPosition(targetPos)
                .Play(() =>
                {
                    onCastEnd?.Invoke();
                });

        target.cc.SetCC(CCType.Wound, 2, true);
    }
}
