using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class EnemyHealth : LivingEntity
{
    private Collider2D coll;
    private SpriteRenderer sr;

    [HideInInspector]
    public EnemyIndicator indicator;

    public Image weaknessImg;

    [Header("���� ����")]
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

    protected override void Die()
    {
        sr.DOFade(0f, 1f)
            .SetEase(Ease.Linear)
            .OnComplete(() => gameObject.SetActive(false));

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
