using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_Gentle_Breeze : SkillPiece
{

    public override void Cast(LivingEntity target, Action onCastEnd = null) //체력을 20 회복한 후 자연필드를 생성한다.
    {
        owner.GetComponent<PlayerHealth>().Heal(value);
        GameManager.Instance.battleFieldHandler.SetFieldType(PatternType.Clover);

        Anim_M_Recover effect = PoolManager.GetItem<Anim_M_Recover>();
        effect.transform.position = owner.transform.position;

        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }
}
