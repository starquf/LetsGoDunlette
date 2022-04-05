using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_NaturalHealing : SkillPiece
{

    public override void Cast(LivingEntity target, Action onCastEnd = null) //체력을 40 회복한다.
    {
        owner.GetComponent<LivingEntity>().Heal(value);

        Anim_M_Recover effect = PoolManager.GetItem<Anim_M_Recover>();
        effect.transform.position = owner.transform.position;

        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }
}
