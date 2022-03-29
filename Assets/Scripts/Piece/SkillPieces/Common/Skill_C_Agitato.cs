using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_C_Agitato : SkillPiece
{
    private BattleHandler bh;

    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;

        hasTarget = true;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        int damageStack = (int)bh.mainRullet.RulletSpeed / 50;

        target.GetDamage(damageStack * 5, currentType);

        onCastEnd?.Invoke();
    }
}
