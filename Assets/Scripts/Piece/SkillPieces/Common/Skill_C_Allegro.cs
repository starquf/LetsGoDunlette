using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_C_Allegro : SkillPiece
{
    private BattleHandler bh;

    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;

        hasTarget = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
        textEffect.SetType(TextUpAnimType.Damage);
        textEffect.transform.position = bh.mainRullet.transform.position;
        textEffect.Play("∑Í∑ø º”µµ ¡ı∞°!");

        owner.GetComponent<LivingEntity>().AddShield(value);
        bh.mainRullet.RulletSpeed += 100f;

        onCastEnd?.Invoke();
    }
}
