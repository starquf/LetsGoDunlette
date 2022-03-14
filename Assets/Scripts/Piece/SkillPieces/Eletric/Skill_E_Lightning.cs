using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_E_Lightning : SkillPiece
{
    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        target.GetDamage(value);

        Anim_E_LightningRod effect = PoolManager.GetItem<Anim_E_LightningRod>();
        effect.transform.position = skillImg.transform.position;
        effect.SetScale(0.5f);

        effect.Play(() =>
        {
            onCastEnd?.Invoke();

            PlayerHealth playerHealth = owner.GetComponent<PlayerHealth>();
            if (playerHealth.HasShield())
            {
                if (Random.Range(0, 100) < 60)
                {
                    playerHealth.cc.SetCC(CCType.Stun, 1);
                }
                else
                {
                    target.cc.SetCC(CCType.Stun, 1);
                }
            }
            else
            {
                target.cc.SetCC(CCType.Stun, 1);
            }
        });

    }
}
