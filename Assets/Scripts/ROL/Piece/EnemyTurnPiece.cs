using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnPiece : RulletPiece
{
    // 적이 걸렸을 때
    public override int Cast()
    {
        GameManager.Instance.battleHandler.EnemyAttack();

        return value;
    }
}
