using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnPiece : RulletPiece
{
    // 플레이어가 걸렸을 때
    public override int Cast()
    {
        GameManager.Instance.battleHandler.PlayerAttack();

        return value;
    }
}
