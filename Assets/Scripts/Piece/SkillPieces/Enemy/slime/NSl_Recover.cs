using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSl_Recover : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        //print($"적 스킬 발동!! 이름 : {PieceName}");
        GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

        Vector3 targetPos = owner.transform.position;

        Anim_M_Recover recoverEffect = PoolManager.GetItem<Anim_M_Recover>();
        recoverEffect.transform.position = targetPos;

        recoverEffect.Play(() =>
        {
            onCastEnd?.Invoke();
        });

        owner.GetComponent<EnemyHealth>().Heal(value);
    }
}
