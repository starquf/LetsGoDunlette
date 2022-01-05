using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyHealth : LivingEntity
{
    private SpriteRenderer sr;
    public EnemyReward enemyReward;

    protected override void Start()
    {

        sr = GetComponent<SpriteRenderer>();
        enemyReward = GetComponent<EnemyReward>();

        BattleHandler battleHandler = GameManager.Instance.battleHandler;

        hpBar = battleHandler.hpBar;
        hpText = battleHandler.hpText;
        damageTrans = battleHandler.damageTrans;
        base.Start();
    }

    protected override void Die()
    {
        sr.DOFade(0f, 1f)
            .SetEase(Ease.Linear);

        //enemyReward.GiveReward();
    }

    public override void Revive()
    {
        base.Revive();

        sr.DOFade(1f, 1f)
            .SetEase(Ease.Linear);
    }
}
