using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_F_Soot : SkillPiece
{
    protected override void Start()
    {
        base.Start();
        bh = GameManager.Instance.battleHandler;
    }
    public override void Cast(LivingEntity target, Action onCastEnd = null) //인벤토리에 <sprite=3>속성 조각이 있을 경우 다음 조각을 <sprite=3>속성 조각으로 강제한다.
    {
        target.GetDamage(GetDamageCalc(Value), currentType);
        onCastEnd?.Invoke();
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(Value)}");
        return desInfos;
    }
}
