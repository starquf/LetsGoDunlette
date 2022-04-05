using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FA_Attack : SkillPiece
{
    private Action<SkillPiece> onNextTurn;

    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void OnRullet()
    {
        base.OnRullet();
        BattleHandler bh = GameManager.Instance.battleHandler;

        bh.battleEvent.RemoveNextSkill(onNextTurn);

        onNextTurn = piece =>
        {
            pieceDes = StringFormatUtil.GetEnemyAttackString(Value);
        };

        bh.battleEvent.SetNextSkill(onNextTurn);
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "АјАн").OnEnd(() =>
        {
            target.GetDamage(Value, this, owner);
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
