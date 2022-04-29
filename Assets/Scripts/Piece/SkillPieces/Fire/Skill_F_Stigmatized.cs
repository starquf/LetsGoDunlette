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

        hasTarget = true;
    }

    public override void OnRullet()
    {
        base.OnRullet();
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        target.GetDamage(Value, currentType);

        GameManager.Instance.battleHandler.fieldHandler.SetFieldType(ElementalType.Fire);

        Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
        textEffect.SetType(TextUpAnimType.Up);
        textEffect.transform.position = target.transform.position;
        textEffect.SetScale(0.7f);
        textEffect.Play("필드변경!");

        animHandler.GetAnim(AnimName.F_ManaSphereHit)
                .SetPosition(target.transform.position)
                .SetScale(0.7f)
                .Play(() =>
                {
                    onCastEnd?.Invoke();
                });
    }
}
