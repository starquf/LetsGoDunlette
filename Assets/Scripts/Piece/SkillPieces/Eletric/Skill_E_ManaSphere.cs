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

        EffectObj staticEffect = PoolManager.GetItem<EffectObj>();
        staticEffect.transform.position = startPos;
        staticEffect.SetSprite(effectSpr);
        staticEffect.SetColorGradient(effectGradient);
        //staticEffect.targetPos = target;

        staticEffect.Play(target, ()=> {
            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
            GameManager.Instance.battleHandler.enemy.GetDamage(Value);

            onCastEnd?.Invoke();

            staticEffect.EndEffect();
        }, BezierType.Linear, isRotate: true);

        GameManager.Instance.battleHandler.enemy.GetDamage(Value);
        //StartCoroutine(EffectCast());
    }
}
