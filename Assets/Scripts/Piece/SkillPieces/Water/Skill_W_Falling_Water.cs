using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_W_Falling_Water : SkillPiece
{
    protected override void Start()
    {
        base.Start();
        bh = GameManager.Instance.battleHandler;
    }
    public override void Cast(LivingEntity target, Action onCastEnd = null) 
    {
        animHandler.GetAnim(AnimName.T_WaterSplash09)
                .SetPosition(target.transform.position)
                .SetScale(1.5f)
                .Play(() =>
                {
                    target.GetDamage(GetDamageCalc(Value), currentType);
                    onCastEnd?.Invoke();
                });
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(Value)}");
        return desInfos;
    }
}
