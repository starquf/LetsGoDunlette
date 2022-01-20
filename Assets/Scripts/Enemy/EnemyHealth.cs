using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyHealth : LivingEntity
{
    private Collider2D coll;
    private SpriteRenderer sr;

    [Header("보스 여부")]
    public bool isBoss = false;

    protected override void Start()
    {
        coll = GetComponent<Collider2D>();
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

        coll.enabled = false;

        GameManager.Instance.inventoryHandler.RemoveAllOwnerPiece(GetComponent<Inventory>());
    }

    public override void Revive()
    {
        base.Revive();

        sr.DOFade(1f, 1f)
            .SetEase(Ease.Linear);

        coll.enabled = true;
    }
}
