using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_F_ChainExplosion : SkillPiece
{

    public override void Cast(Action onCastEnd = null)
    {
        base.Cast();
        print($"스킬 발동!! 이름 : {PieceName}");

        Vector3 target = GameManager.Instance.battleHandler.enemy.transform.position;
        Vector3 startPos = GameManager.Instance.battleHandler.player.transform.position;

        GameManager.Instance.battleHandler.enemy.GetDamage(Value);
        //StartCoroutine(EffectCast());
    }
}
