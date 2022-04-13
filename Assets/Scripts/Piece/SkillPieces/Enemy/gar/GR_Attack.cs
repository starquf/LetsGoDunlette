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
        SetIndicator(owner.gameObject, "АјАн").OnEnd(() =>
        {
            target.GetDamage(Value, this, owner);
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            Anim_N_ManaSphereHit hitEffect = PoolManager.GetItem<Anim_N_ManaSphereHit>();
            hitEffect.transform.position = GameManager.Instance.enemyEffectTrm.position; hitEffect.SetScale(1.5f);

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }
}
