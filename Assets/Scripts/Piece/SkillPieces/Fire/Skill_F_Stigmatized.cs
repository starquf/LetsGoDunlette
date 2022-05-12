using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_F_Stigmatized : SkillPiece
{
    private BattleHandler bh = null;
    SkillEvent eventInfo = null;

    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;

        isTargeting = true;
    }

    public override void OnRullet()
    {
        base.OnRullet();
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        target.GetDamage(Value, currentType);

        GameManager.Instance.battleHandler.fieldHandler.SetFieldType(ElementalType.Fire);

        animHandler.GetTextAnim()
        .SetType(TextUpAnimType.Up)
        .SetPosition(target.transform.position)
        .SetScale(0.7f)
        .Play("�ʵ庯��!");

        animHandler.GetAnim(AnimName.F_ManaSphereHit)
                .SetPosition(target.transform.position)
                .SetScale(0.7f)
                .Play(() =>
                {
                    onCastEnd?.Invoke();
                });
    }
}
