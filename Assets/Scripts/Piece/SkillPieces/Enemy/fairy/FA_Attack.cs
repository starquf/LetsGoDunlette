using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FA_Attack : SkillPiece
{
    private Action<SkillPiece,Action> onNextTurn;
    SkillEvent eventInfo = null;

    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void OnRullet()
    {
        base.OnRullet();
        bh.battleEvent.RemoveEventInfo(eventInfo);

        onNextTurn = (piece,action) =>
        {
            pieceDes = StringFormatUtil.GetEnemyAttackString(Value);

            action?.Invoke();
        };

        eventInfo = new SkillEvent(EventTimeSkill.AfterSkill, onNextTurn);
        bh.battleEvent.BookEvent(eventInfo);
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "АјАн").OnEndAction(() =>
        {
            target.GetDamage(Value, this, Owner);
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }
}
