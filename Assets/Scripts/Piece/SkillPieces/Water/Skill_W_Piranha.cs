using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_W_Piranha : SkillPiece
{
    public override void Cast(LivingEntity target, Action onCastEnd = null) //��󿡰� <sprite=12>�� ���� �� <sprite=12>5 �ο�
    {
        if (target.cc.IsCC(CCType.Wound))
        {
            target.cc.IncreaseCCTurn(CCType.Wound, 5);
        }

        target.GetDamage(GetDamageCalc(value));
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{Value}");
        return desInfos;
    }
}
