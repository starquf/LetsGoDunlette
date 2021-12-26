using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_C_Sign : SkillPiece
{
    public override void Cast(Action onCastEnd = null)
    {
        BattleHandler bh = GameManager.Instance.battleHandler;

        // Ä§¹¬ »óÅÂ¸é
        if (CheckSilence())
        {
            Anim_Contract contractAnim = PoolManager.GetItem<Anim_Contract>();
            contractAnim.transform.position = bh.player.transform.position;

            contractAnim.Play(() => {
                onCastEnd?.Invoke();
            });

            bh.SetContract(Value, 3);
        }
        else
        {
            onCastEnd?.Invoke();
        }
    }
}
