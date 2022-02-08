using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_E_ManaSphere : SkillPiece
{
    public Sprite effectSpr; 
    private Gradient effectGradient;

    protected override void Start()
    {
        base.Start();
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[PatternType.Diamonds];
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        print($"스킬 발동!! 이름 : {PieceName}");

        Vector3 targetPos = target.transform.position;
        Vector3 startPos = owner.transform.position;

        EffectObj skillEffect = PoolManager.GetItem<EffectObj>();
        skillEffect.transform.position = startPos;
        skillEffect.SetSprite(effectSpr);
        skillEffect.SetColorGradient(effectGradient);

        skillEffect.Play(targetPos, () => {
            Anim_E_ManaSphereHit hitEffect = PoolManager.GetItem<Anim_E_ManaSphereHit>();
            hitEffect.transform.position = targetPos;

            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
            target.GetDamage(Value, patternType);
            onCastEnd?.Invoke();

            hitEffect.Play(() =>
            {
            });

            skillEffect.EndEffect();
        }, BezierType.Linear, isRotate: true);
    }
}
