using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_C_Sign : SkillPiece
{
    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {

        owner.GetComponent<LivingEntity>().cc.SetBuff(BuffType.Contract, 20);
    }
}
