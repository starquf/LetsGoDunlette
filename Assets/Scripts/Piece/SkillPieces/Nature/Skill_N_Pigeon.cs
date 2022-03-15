using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_Pigeon : SkillPiece
{

    public override void Cast(LivingEntity target, Action onCastEnd = null) //������ 60���ظ� ������.		ħ��	-	������ 2�ϰ� ħ���� �ο��Ѵ�.
    {
        target.GetDamage(value);

        Anim_F_Arson effect = PoolManager.GetItem<Anim_F_Arson>();
        effect.transform.position = skillImg.transform.position;
        effect.SetScale(0.5f);

        effect.Play(() =>
        {
            target.cc.SetCC(CCType.Silence, 2);
            onCastEnd?.Invoke();
        });
    }
}
