using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_W_ManaSphere : SkillPiece
{
    public Sprite manaSphereSpr;
    private Gradient effectGradient;

    protected override void Start()
    {
        base.Start();
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[PatternType.Spade];
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        //print($"스킬 발동!! 이름 : {PieceName}");

        Vector3 targetPos = target.transform.position;
        Vector3 startPos = owner.transform.position;

        EffectObj skillEffect = PoolManager.GetItem<EffectObj>();
        skillEffect.transform.position = startPos;
        skillEffect.SetSprite(manaSphereSpr);
        skillEffect.SetColorGradient(effectGradient);
        skillEffect.SetScale(Vector3.one);

        skillEffect.Play(targetPos, () => {
            Anim_W_ManaSphereHit hitEffect = PoolManager.GetItem<Anim_W_ManaSphereHit>();
            hitEffect.transform.position = targetPos;

            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
            target.GetDamage(Value, currentType);
            onCastEnd?.Invoke();

            hitEffect.Play();

            skillEffect.EndEffect();
        }, BezierType.Linear, isRotate: true, playSpeed:2f);
    }
}
