using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_W_Change : SkillPiece
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
        owner.GetComponent<PlayerHealth>().ChangeShieldToHealth();

        Anim_M_Recover recoverEffect = PoolManager.GetItem<Anim_M_Recover>();
        recoverEffect.transform.position = owner.transform.position;
        recoverEffect.SetScale(0.5f);

        recoverEffect.Play();

        animHandler.GetAnim(AnimName.W_Splash01)
            .SetPosition(owner.GetComponent<LivingEntity>().hpBar.transform.position)
            .SetScale(0.5f)
            .Play(() => onCastEnd?.Invoke());
    }

}
