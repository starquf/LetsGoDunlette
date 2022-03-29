using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_C_Double_edged_sword : SkillPiece
{
    
    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {

        PlayerHealth playerHealth = owner.GetComponent<PlayerHealth>();
        if (!playerHealth.HasShield())
        {
            playerHealth.GetDamage(20);
        }
        else
        {
            target.GetDamage(value, currentType);
        }


        Anim_C_SphereCast effect = PoolManager.GetItem<Anim_C_SphereCast>();
        effect.transform.position = skillImg.transform.position;
        effect.SetScale(0.5f);

        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }
}
