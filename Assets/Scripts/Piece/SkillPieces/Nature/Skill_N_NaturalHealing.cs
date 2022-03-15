using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_NaturalHealing : SkillPiece
{

    public override void Cast(LivingEntity target, Action onCastEnd = null) //체력을 40 회복한다.
    {
        owner.GetComponent<PlayerHealth>().Heal(40);

        Anim_F_Arson effect = PoolManager.GetItem<Anim_F_Arson>();
        effect.transform.position = skillImg.transform.position;
        effect.SetScale(0.5f);


        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }
}
