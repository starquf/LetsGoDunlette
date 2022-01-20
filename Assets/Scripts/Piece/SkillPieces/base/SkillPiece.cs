using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class SkillPiece : RulletPiece
{
    public bool isPlayerSkill = true;
    public bool isInRullet = false;

    [HideInInspector]
    public Inventory owner;

    protected virtual void Awake()
    {
        PieceType = PieceType.SKILL;
    }

    public override void Cast(LivingEntity targetTrm, Action onCastEnd = null)
    {

    }

    public virtual bool CheckSilence()
    {
        CrowdControl cc = owner.GetComponent<CrowdControl>();

        // 침묵 상태인가?
        return cc.ccDic[CCType.Silence] > 0;
    }
}
