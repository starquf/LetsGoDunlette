using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MI_Attack : SkillPiece
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
            target.GetDamage(Value, owner.gameObject);
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            Anim_M_Bite hitEffect = PoolManager.GetItem<Anim_M_Bite>();
            hitEffect.transform.position = owner.transform.position;

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }
}
