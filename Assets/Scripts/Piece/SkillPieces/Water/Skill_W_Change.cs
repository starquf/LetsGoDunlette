using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_W_Change : SkillPiece
{

    protected override void Start()
    {
        base.Start();

        hasTarget = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        owner.GetComponent<PlayerHealth>().ChangeShieldToHealth();
        Anim_W_Splash splashEffect = PoolManager.GetItem<Anim_W_Splash>();
        splashEffect.transform.position = skillImg.transform.position;
        splashEffect.SetScale(0.5f);

        splashEffect.Play(() => onCastEnd?.Invoke());
    }

}
