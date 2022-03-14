using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_F_TickTock : SkillPiece
{

    public override void Cast(LivingEntity target, Action onCastEnd = null) //룰렛에 들어온 뒤 사용되지 않은채로 3턴이 지나면 자신에게 60의 데미지를 준 뒤 무덤으로 이동한다.
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
