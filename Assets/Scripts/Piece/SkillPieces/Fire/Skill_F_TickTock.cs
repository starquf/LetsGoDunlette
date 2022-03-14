using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_F_TickTock : SkillPiece
{

    public override void Cast(LivingEntity target, Action onCastEnd = null) //�귿�� ���� �� ������ ����ä�� 3���� ������ �ڽſ��� 60�� �������� �� �� �������� �̵��Ѵ�.
    {
        BattleHandler bh = GameManager.Instance.battleHandler;

        target.GetDamage(value);

        Anim_F_Arson effect = PoolManager.GetItem<Anim_F_Arson>();
        effect.transform.position = skillImg.transform.position;
        effect.SetScale(0.5f);

        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });

    }
}
