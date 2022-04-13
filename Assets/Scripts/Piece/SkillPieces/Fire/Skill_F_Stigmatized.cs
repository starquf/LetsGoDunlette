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

        GameManager.Instance.battleHandler.fieldHandler.SetFieldType(PatternType.Heart);

        Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
        textEffect.SetType(TextUpAnimType.Up);
        textEffect.transform.position = target.transform.position;
        textEffect.SetScale(0.7f);
        textEffect.Play("필드변경!");

        Anim_F_ManaSphereHit hitEffect = PoolManager.GetItem<Anim_F_ManaSphereHit>();
        hitEffect.transform.position = target.transform.position;
        hitEffect.SetScale(0.7f);
        hitEffect.Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }
}
