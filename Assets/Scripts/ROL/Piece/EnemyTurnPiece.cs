using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnPiece : RulletPiece
{
    // ���� �ɷ��� ��
    public override int Cast()
    {
        GameManager.Instance.battleHandler.EnemyAttack();

        return value;
    }
}
