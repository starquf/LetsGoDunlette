using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_F_Arson : SkillPiece
{

    public override void Cast(Action onCastEnd = null)
    {
        BattleHandler bh = GameManager.Instance.battleHandler;
        base.Cast();
        print($"스킬 발동!! 이름 : {PieceName}");

        Vector3 target = bh.enemy.transform.position;

        Anim_F_Arson staticEffect = PoolManager.GetItem<Anim_F_Arson>();
        staticEffect.transform.position = target;

        staticEffect.Play(() => {
            bh.enemy.GetDamage(Value);
            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

            onCastEnd?.Invoke();
        });
    }
}
