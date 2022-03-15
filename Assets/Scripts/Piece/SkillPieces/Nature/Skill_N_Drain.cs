using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_Drain : SkillPiece
{

    public override void Cast(LivingEntity target, Action onCastEnd = null) //적에게 50의 피해를 입힌다.		-	회복	체력을 20 회복한다.
    {
        target.GetDamage(value);
        owner.GetComponent<PlayerHealth>().Heal(20);

        Anim_F_Arson effect = PoolManager.GetItem<Anim_F_Arson>();
        effect.transform.position = skillImg.transform.position;
        effect.SetScale(0.5f);


        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }
}
