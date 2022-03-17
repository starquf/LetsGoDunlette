using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_NaturalHealing : SkillPiece
{

    public override void Cast(LivingEntity target, Action onCastEnd = null) //ü���� 40 ȸ���Ѵ�.
    {
        owner.GetComponent<PlayerHealth>().Heal(40);

        Anim_M_Recover effect = PoolManager.GetItem<Anim_M_Recover>();
        effect.transform.position = GameManager.Instance.battleHandler.playerImgTrans.position;

        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }
}
