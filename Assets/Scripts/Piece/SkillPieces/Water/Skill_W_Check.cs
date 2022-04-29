using System;
using UnityEngine;

public class Skill_W_Check : SkillPiece
{
    [Header("½¯µå·®")]
    public int shieldValue = 40;

    protected override void Start()
    {
        base.Start();
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        target.GetDamage(value, currentType);

        animHandler.GetAnim(AnimName.W_Splash01)
            .SetPosition(skillImg.transform.position)
            .SetScale(0.5f)
            .Play(() =>
            {
                if (target.cc.IsCC())
                {
                    animHandler.GetAnim(AnimName.W_Splash01)
                    .SetPosition(skillImg.transform.position)
                    .SetScale(0.5f)
                    .Play(() => 
                    {
                        owner.GetComponent<PlayerHealth>().AddShield(shieldValue);
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
