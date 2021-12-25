using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHealth : LivingEntity
{
    public CrowdControl cc;

    public Color damageColor;
    public Image damageEffect;

    private void Awake()
    {
        cc = GetComponent<CrowdControl>();
    }

    protected override void Die()
    {
        print("³ª Á×À½!");
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);

        damageEffect.color = damageColor;
        damageEffect.DOFade(0f, 0.55f);
    }
}
