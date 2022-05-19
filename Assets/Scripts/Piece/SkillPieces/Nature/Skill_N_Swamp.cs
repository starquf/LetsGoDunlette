using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_N_Swamp : SkillPiece
{
    public Sprite drainingEffectSpr;
    private Gradient effectGradient;

    protected override void Start()
    {
        base.Start();

        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.Nature];

        isTargeting = true;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc()}");
        desInfos[1].SetInfo(DesIconType.Exhausted, $"4");
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) 
    {
        N_Swamp(target, onCastEnd);
    }

    private void N_Swamp(LivingEntity target, Action onCastEnd = null)
    {
        target.GetDamage(GetDamageCalc());

        GameManager.Instance.cameraHandler.ShakeCamera(2f, 0.3f);

        target.cc.SetCC(CCType.Exhausted, 4);
    }
}
