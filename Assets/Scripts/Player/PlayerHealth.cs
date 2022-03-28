using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHealth : LivingEntity
{
    public Color damageBGColor;
    public Image damageBGEffect;

    protected override void Die()
    {
        //print("³ª Á×À½!");
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);

        damageBGEffect.color = damageBGColor;
        damageBGEffect.DOFade(0f, 0.55f);
    }
}
