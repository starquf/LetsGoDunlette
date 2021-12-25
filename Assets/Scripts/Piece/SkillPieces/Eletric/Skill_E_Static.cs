using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_E_Static : SkillPiece
{
    public GameObject staticEffectPrefab;

    public override void Cast(Action onCastEnd = null)
    {
        base.Cast();
        print($"스킬 발동!! 이름 : {PieceName}");

        Vector3 target = GameManager.Instance.battleHandler.enemy.transform.position;
        target.y -= 0.7f;

        Anim_Static staticEffect = PoolManager.GetItem<Anim_Static>();
        staticEffect.transform.position = target;

        staticEffect.Play(() => {
            GameManager.Instance.battleHandler.enemy.GetDamage(Value);
            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

            onCastEnd?.Invoke();
        });
    }
}
