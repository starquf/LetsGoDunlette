using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_Leech : SkillPiece
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

    public override void Cast(LivingEntity target, Action onCastEnd = null) //대상의 <sprite=12>만큼 <sprite=0>속성 추가 피해를 준다.
    {
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
        target.GetDamage(target.cc.GetCCValue(CCType.Wound), this, Owner);

        animHandler.GetAnim(AnimName.M_Butt)
                .SetPosition(target.transform.position)
                .SetScale(1f)
                .Play(()=>
                {
                    onCastEnd?.Invoke();
                });
    }
}
