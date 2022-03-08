using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class EnemyHealth : LivingEntity
{
    private Collider2D coll;
    private SpriteRenderer sr;

    [HideInInspector]
    public EnemyIndicator indicator;

    public Image weaknessImg;

    [Header("보스 여부")]
    public bool isBoss = false;

    protected override void Start()
    {
        coll = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        indicator = GetComponent<EnemyIndicator>();

        if(weaknessImg != null)
            weaknessImg.sprite = GameManager.Instance.inventoryHandler.effectSprDic[weaknessType];

        base.Start();
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);

        sr.color = Color.red;
        sr.DOColor(Color.white, 0.35f);
    }

    protected override void Die()
    {
        Action onDieAction = () => { };

        onDieAction = () =>
        {
            ShowDieEffect();
            bh.onEndAttack -= onDieAction;
        };

        bh.onEndAttack += onDieAction;

        GameManager.Instance.inventoryHandler.RemoveAllOwnerPiece(GetComponent<Inventory>());
        coll.enabled = false;

        bh.enemys.Remove(this);
    }

    public virtual void ShowDieEffect()
    {
        sr.DOFade(0f, 1f)
            .SetEase(Ease.Linear)
            .OnComplete(() => gameObject.SetActive(false));
    }

    public override void Revive()
    {
        base.Revive();

        sr.DOFade(1f, 1f)
            .SetEase(Ease.Linear);

        coll.enabled = true;
    }
}
