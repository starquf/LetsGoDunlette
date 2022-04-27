using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class F_FieldHandler : FieldHandler
{
    [SerializeField]
    private SpriteRenderer DistortionEffectSpr;
    [SerializeField]
    private SpriteRenderer redTransSpr;

    private Color redTransColor;

    private void Awake()
    {
        redTransColor = redTransSpr.color;

        fieldType = ElementalType.Fire;
    }

    public override void DisableField(bool skip = false)
    {
        base.DisableField();
        if(skip)
        {
            redTransSpr.color = new Color(redTransColor.r, redTransColor.g, redTransColor.b, 0);
            DistortionEffectSpr.color = new Color(DistortionEffectSpr.color.r, DistortionEffectSpr.color.g, DistortionEffectSpr.color.b, 0);
        }
        else
        {
            redTransSpr.DOKill();
            redTransSpr.DOFade(0, 0.5f);
            DistortionEffectSpr.DOFade(0, 0.5f);
        }
    }

    public override void EnableField(bool skip = false)
    {
        base.EnableField();
        if (skip)
        {
            redTransSpr.color = redTransColor;
            DistortionEffectSpr.color = Color.white;
        }
        else
        {
            redTransSpr.DOKill();
            redTransSpr.DOFade(redTransColor.a, 0.5f);
            DistortionEffectSpr.DOFade(1, 0.5f);
        }
    }
}
