using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_ManaSphere : SkillPiece
{
    public Sprite manaSphereSpr; 
    private Gradient effectGradient;

    protected override void Start()
    {
        base.Start();
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[PatternType.Clover];
    }

    public override void Cast(Action onCastEnd = null)
    {
        base.Cast();
        print($"스킬 발동!! 이름 : {PieceName}");

        Vector3 target = GameManager.Instance.battleHandler.enemy.transform.position;
        Vector3 startPos = GameManager.Instance.battleHandler.player.transform.position;

        EffectObj skillEffect = PoolManager.GetItem<EffectObj>();
        skillEffect.transform.position = startPos;
        skillEffect.SetSprite(manaSphereSpr);
        skillEffect.SetColorGradient(effectGradient);

        skillEffect.Play(target, () => {
            Anim_N_ManaSphereHit hitEffect = PoolManager.GetItem<Anim_N_ManaSphereHit>();
            hitEffect.transform.position = target;

            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
            GameManager.Instance.battleHandler.enemy.GetDamage(Value);
            onCastEnd?.Invoke();

            hitEffect.Play(() =>
            {
            });

            skillEffect.EndEffect();
        }, BezierType.Linear, isRotate: true);
    }
}
