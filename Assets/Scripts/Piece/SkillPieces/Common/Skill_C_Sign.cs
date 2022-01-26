using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_C_Sign : SkillPiece
{
    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        // 침묵 상태면
        Anim_Contract contractAnim = PoolManager.GetItem<Anim_Contract>();
        contractAnim.transform.position = owner.transform.position;

        contractAnim.Play(() => {
            onCastEnd?.Invoke();
        });

        owner.GetComponent<LivingEntity>().cc.SetBuff(BuffType.Contract, 20);
    }
}
