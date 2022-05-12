using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SR_Attack : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "АјАн").OnEndAction(() =>
        {
            target.GetDamage(Value, this, Owner);
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

             animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
             .SetScale(2)
             .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }
}
