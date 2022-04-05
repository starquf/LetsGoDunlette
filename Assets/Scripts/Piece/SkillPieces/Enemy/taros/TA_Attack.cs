using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TA_Attack : SkillPiece
{
    private int firstValue;
    private Action<SkillPiece> onNextTurn;

    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
        firstValue = Value;

        pieceDes = StringFormatUtil.GetEnemyAttackString(Value);
    }

    public override void OnRullet()
    {
        base.OnRullet();
        BattleHandler bh = GameManager.Instance.battleHandler;

        bh.battleEvent.RemoveNextSkill(onNextTurn);

        onNextTurn = piece =>
        {
            if (owner.GetComponent<EnemyHealth>().cc.IsCC(CCType.Heating))
            {
                SetValue(firstValue + 10);
            }
            else
            {
                SetValue(firstValue);
            }

            pieceDes = StringFormatUtil.GetEnemyAttackString(Value);
        };

        bh.battleEvent.SetNextSkill(onNextTurn);
    }

    public override void ResetPiece()
    {
        base.ResetPiece();

        GameManager.Instance.battleHandler.battleEvent.RemoveNextSkill(onNextTurn);
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "����").OnEnd(() =>
        {
            if(owner.GetComponent<EnemyHealth>().cc.IsCC(CCType.Heating))
            {
                target.GetDamage(Value, this, owner);
            }
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            Anim_M_Sword hitEffect = PoolManager.GetItem<Anim_M_Sword>();
            hitEffect.transform.position = GameManager.Instance.enemyEffectTrm.position; hitEffect.SetScale(2);

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }
}
