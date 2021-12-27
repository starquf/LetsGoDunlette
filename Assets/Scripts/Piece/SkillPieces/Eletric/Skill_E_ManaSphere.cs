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

    public override void Cast(Action onCastEnd = null)
    {
        base.Cast();
        print($"��ų �ߵ�!! �̸� : {PieceName}");

        Vector3 target = GameManager.Instance.battleHandler.enemy.transform.position;
        Vector3 startPos = GameManager.Instance.battleHandler.player.transform.position;

        EffectObj skillEffect = PoolManager.GetItem<EffectObj>();
        skillEffect.transform.position = startPos;
        skillEffect.SetSprite(effectSpr);
        skillEffect.SetColorGradient(effectGradient);

        skillEffect.Play(target, () => {
            Anim_E_ManaSphereHit hitEffect = PoolManager.GetItem<Anim_E_ManaSphereHit>();
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
