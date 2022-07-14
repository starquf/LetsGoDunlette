using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_Aggravation : SkillPiece
{

    protected override void Start()
    {
        base.Start();
        bh = GameManager.Instance.battleHandler;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) //����� <sprite=12>�� 2��� �ø���.
    {
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
        target.cc.SetCC(CCType.Wound, target.cc.GetCCValue(CCType.Wound));
    }
}
