using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSW_Old_Shield : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        print($"적 스킬 발동!! 이름 : {PieceName}");
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

        Vector3 targetPos = owner.transform.position;

        Anim_M_Shield shieldEffect = PoolManager.GetItem<Anim_M_Shield>();
        shieldEffect.transform.position = targetPos;

        shieldEffect.Play(() =>
        {
            onCastEnd?.Invoke();
        });

        owner.GetComponent<EnemyHealth>().AddShield(value);
    }
}
