using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_C_ManaSphere : SkillPiece
{
    public Sprite manaSphereSpr;

    private Gradient effectGradient;

    protected override void Start()
    {
        base.Start();
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[PatternType.None];
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        Vector3 startPos = owner.transform.position;
        Vector3 targetPos = target.transform.position;

        Anim_C_SphereCast castAnim = PoolManager.GetItem<Anim_C_SphereCast>();
        castAnim.transform.position = startPos;

        castAnim.Play(() =>
        {
            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.transform.position = startPos;
            effect.SetSprite(manaSphereSpr);
            effect.SetColorGradient(effectGradient);

            effect.Play(targetPos, () =>
            {
                Anim_C_ManaSphereHit hitEffect = PoolManager.GetItem<Anim_C_ManaSphereHit>();
                hitEffect.transform.position = targetPos;

                GameManager.Instance.cameraHandler.ShakeCamera(1f, 0.2f);
                target.GetDamage(Value, patternType);
                onCastEnd?.Invoke();

                hitEffect.Play(() =>
                {
                });

                effect.EndEffect();
            }, BezierType.Linear, isRotate:true);
        });
    }
}
