using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_Drain : SkillPiece
{

    public override void Cast(LivingEntity target, Action onCastEnd = null) //������ 50�� ���ظ� ������.		-	ȸ��	ü���� 20 ȸ���Ѵ�.
    {
        target.GetDamage(value);
        owner.GetComponent<PlayerHealth>().Heal(20);

        Anim_F_Arson effect = PoolManager.GetItem<Anim_F_Arson>();
        effect.transform.position = skillImg.transform.position;
        effect.SetScale(0.5f);


        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }
}
