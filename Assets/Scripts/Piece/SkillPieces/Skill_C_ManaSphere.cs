using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_C_ManaSphere : SkillPiece
{
    public GameObject animObj;
    public Sprite manaSphereSpr;

    public override void Cast(Action onCastEnd = null)
    {
        Vector3 startPos = GameManager.Instance.battleHandler.player.transform.position;
        Vector3 target = GameManager.Instance.battleHandler.enemy.transform.position;

        EffectObj effect = PoolManager.GetItem<EffectObj>();
        effect.transform.position = startPos;
        effect.SetSprite(manaSphereSpr);

        effect.Play(target, () =>
        {
            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
            GameManager.Instance.battleHandler.enemy.GetDamage(Value);

            onCastEnd?.Invoke();

            effect.EndEffect();

        }, BezierType.Quadratic);

        /*
        AnimObj a = Instantiate(animObj, target, Quaternion.identity).GetComponent<AnimObj>();
        a.Play("normalHitEffect01", () => {
            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
            onCastEnd?.Invoke();
        });
        */

    }
}
