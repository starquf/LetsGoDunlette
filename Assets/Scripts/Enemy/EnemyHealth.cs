using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyHealth : LivingEntity
{
    private SpriteRenderer sr;

    [Header("보스 여부")]
    public bool isBoss = false;

    protected override void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        BattleHandler battleHandler = GameManager.Instance.battleHandler;

        hpBar = battleHandler.hpBar;
        hpShieldBar = battleHandler.hpShieldBar;
        hpText = battleHandler.hpText;
        damageTrans = battleHandler.damageTrans;

        base.Start();
    }

    protected override void Die()
    {
        sr.DOFade(0f, 1f)
            .SetEase(Ease.Linear);
    }

    public override void Revive()
    {
        base.Revive();

        sr.DOFade(1f, 1f)
            .SetEase(Ease.Linear);
    }
}
