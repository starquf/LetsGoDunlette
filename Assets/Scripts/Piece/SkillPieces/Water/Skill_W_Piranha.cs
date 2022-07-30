using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_W_Piranha : SkillPiece
{
    protected override void Start()
    {
        base.Start();
        bh = GameManager.Instance.battleHandler;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(Value)}");
        return desInfos;
    }
    public override void Cast(LivingEntity target, Action onCastEnd = null) //��󿡰� <sprite=12>�� ���� �� <sprite=12>5 �ο�
    {
        if (target.cc.IsCC(CCType.Wound))
        {
            target.cc.IncreaseCCTurn(CCType.Wound, 5);
        }

        target.GetDamage(GetDamageCalc(Value));
        onCastEnd?.Invoke();
    }
}
