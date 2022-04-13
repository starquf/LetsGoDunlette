using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skil_F_Start_fire : SkillPiece
{
    protected override void Start()
    {
        base.Start();
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) //적에게 80의 피해를 입힌다. //물 필드가 깔려있는 상태라면 데미지가 반으로 감소한다.
    {
        //print($"스킬 발동!! 이름 : {PieceName}");

        Vector3 targetPos = target.transform.position;

        Anim_FireEffect02 hitEffect = PoolManager.GetItem<Anim_FireEffect02>();
        hitEffect.transform.position = targetPos;

        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

        if(GameManager.Instance.battleFieldHandler.CheckFieldType(PatternType.Spade))
        {
            target.GetDamage(Value / 2, currentType);
        }
        else
        {
            target.GetDamage(Value, currentType);
        }

        hitEffect.Play(()=>
        {
            onCastEnd?.Invoke();
        });
    }
}
