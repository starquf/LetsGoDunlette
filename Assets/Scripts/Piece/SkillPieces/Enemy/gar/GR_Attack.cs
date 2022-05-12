using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GR_Attack : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "АјАн").OnEndAction(() =>
        {
            target.GetDamage(Value, this, owner);
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            animHandler.GetAnim(AnimName.N_ManaSphereHit)
                .SetPosition(GameManager.Instance.enemyEffectTrm.position)
                .SetScale(1.5f)
                .Play(() => 
                {
                    onCastEnd?.Invoke();
                });
        });
    }
}
