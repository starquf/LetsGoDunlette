using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnRullet : Rullet
{
    // 3개 18
    public List<RulletPiece> playerTurnPieces = new List<RulletPiece>();
    // 3개 18
    public List<RulletPiece> enemyTurnPieces = new List<RulletPiece>();

    protected override void RulletResult()
    {
        base.RulletResult();

        if (result != null)
        {
            result.Cast();
            result.AddSize(-3);

            result = null;
        }
        else 
        {
            pieces[0].AddSize(3);
        }       
    }

    public void InitTurn()
    {
        pieces[0].ChangeSize(30);
        SetRullet();
        ResetRulletSize();
    }

    // 적이 걸렸을 경우
    protected override void CastDefault()
    {
        GameManager.Instance.battleHandler.EnemyAttack();
    }
}
