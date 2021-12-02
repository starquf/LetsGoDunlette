using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurnRullet : Rullet
{
    // 3�� 18
    public List<RulletPiece> playerTurnPieces = new List<RulletPiece>();
    // 3�� 18
    public List<RulletPiece> enemyTurnPieces = new List<RulletPiece>();
    private RectTransform rectTransform;

    protected override void Start()
    {
        base.Start();
        rectTransform = GetComponent<RectTransform>();
    }

    public override void RollRullet()
    {
        base.RollRullet();

        //rectTransform.DOAnchorPosY(-165f, 1f);
        //transform.DOScale(new Vector3(3, 3, 3), 1f);
    }
    public override void ResetRulletSize()
    {
        base.ResetRulletSize();
        //rectTransform.DOAnchorPosY(0, 1f);
        //transform.DOScale(new Vector3(1, 1, 1), 1f);
    }

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

    // ���� �ɷ��� ���
    protected override void CastDefault()
    {
        GameManager.Instance.battleHandler.EnemyAttack();
    }
}
