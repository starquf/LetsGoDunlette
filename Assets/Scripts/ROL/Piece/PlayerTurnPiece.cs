using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnPiece : RulletPiece
{
    // �÷��̾ �ɷ��� ��
    public override int Cast()
    {
        GameManager.Instance.battleHandler.PlayerAttack();

        return value;
    }
}
