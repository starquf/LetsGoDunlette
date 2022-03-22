using System;
using UnityEngine;

public class Skill_W_Check : SkillPiece
{
    protected override void Start()
    {
        base.Start();
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        target.GetDamage(value);

        Anim_W_Splash splashEffect = PoolManager.GetItem<Anim_W_Splash>();
        splashEffect.transform.position = skillImg.transform.position;
        splashEffect.SetScale(0.5f);

        splashEffect.Play(() =>
        {
            if (target.cc.IsCC())
            {
                Anim_W_Splash splashEffect = PoolManager.GetItem<Anim_W_Splash>();
                splashEffect.transform.position = skillImg.transform.position;
                splashEffect.SetScale(0.5f);

                splashEffect.Play(() =>
                {
                    owner.GetComponent<PlayerHealth>().AddShield(20);
                    onCastEnd?.Invoke();
                });
            }
            else
            {
                onCastEnd?.Invoke();
            }
        });
    }
}
