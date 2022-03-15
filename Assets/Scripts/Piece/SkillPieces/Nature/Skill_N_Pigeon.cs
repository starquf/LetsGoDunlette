using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_Pigeon : SkillPiece
{

    public override void Cast(LivingEntity target, Action onCastEnd = null) //적에게 60피해를 입힌다.		침묵	-	적에게 2턴간 침묵을 부여한다.
    {
        target.GetDamage(value);

        Anim_F_Arson effect = PoolManager.GetItem<Anim_F_Arson>();
        effect.transform.position = skillImg.transform.position;
        effect.SetScale(0.5f);

        effect.Play(() =>
        {
            target.cc.SetCC(CCType.Silence, 2);
            onCastEnd?.Invoke();
        });
    }
}
