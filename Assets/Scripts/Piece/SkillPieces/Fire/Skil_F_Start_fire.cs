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

    public override void Cast(LivingEntity target, Action onCastEnd = null) //������ 80�� ���ظ� ������. //�� �ʵ尡 ����ִ� ���¶�� �������� ������ �����Ѵ�.
    {
        //print($"��ų �ߵ�!! �̸� : {PieceName}");

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
