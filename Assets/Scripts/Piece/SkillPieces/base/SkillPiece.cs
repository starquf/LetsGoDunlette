using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class SkillPiece : RulletPiece
{
    public bool isPlayerSkill = true;

    protected virtual void Awake()
    {
        PieceType = PieceType.SKILL;
    }

    public override void Cast(Action onCastEnd = null)
    {
        PlayerAttackAnimation();
    }

    private void PlayerAttackAnimation()
    {
        Transform playerTrm = GameManager.Instance.battleHandler.player.transform;
        Vector3 enermyPos = GameManager.Instance.battleHandler.enemy.transform.position;
        Vector3 playerPos = playerTrm.position;


        Sequence sequence = DOTween.Sequence().Append(playerTrm.DOMoveX(Mathf.Lerp(playerPos.x, enermyPos.x, 0.3f), 0.1f)).Append(playerTrm.DOMoveX(playerPos.x, 0.2f));
    }
}