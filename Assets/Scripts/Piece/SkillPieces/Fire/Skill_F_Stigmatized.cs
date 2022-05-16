using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_F_Stigmatized : SkillPiece
{
    protected override void Start()
    {
        base.Start();
        isTargeting = true;
    }

    public override void OnRullet()
    {
        base.OnRullet();
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        target.GetDamage(Value, currentType);

        bh.fieldHandler.SetFieldType(ElementalType.Fire);

        animHandler.GetTextAnim()
        .SetType(TextUpAnimType.Up)
        .SetPosition(target.transform.position)
        .SetScale(0.7f)
        .Play("필드변경!");

        animHandler.GetAnim(AnimName.F_ManaSphereHit)
                .SetPosition(target.transform.position)
                .SetScale(0.7f)
                .Play(() =>
                {
                    onCastEnd?.Invoke();
                });
    }
}
