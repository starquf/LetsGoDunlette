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
        animHandler.GetTextAnim()
        .SetType(TextUpAnimType.Up)
        .SetPosition(bh.mainRullet.transform.position)
        .Play("�귿 �ӵ� ����!");

        owner.GetComponent<LivingEntity>().AddShield(value);
        bh.mainRullet.RulletSpeed += 100f;

        onCastEnd?.Invoke();
    }
}
