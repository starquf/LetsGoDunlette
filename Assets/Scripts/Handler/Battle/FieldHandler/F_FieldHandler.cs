using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class F_FieldHandler : FieldHandler
{
    [SerializeField]
    private GameObject DistortionEffectObj;
    [SerializeField]
    private SpriteRenderer redTransSpr;

    private Color redTransColor;

    private void Awake()
    {
        redTransColor = redTransSpr.color;
    }

    public override void DisableField(bool skip = false)
    {
        base.DisableField();
        if(skip)
        {
            redTransSpr.color = new Color(redTransColor.r, redTransColor.g, redTransColor.b, 0);
            DistortionEffectObj.SetActive(false);
        }
        else
        {
            redTransSpr.DOKill();
            redTransSpr.DOFade(0, 0.5f);
            DistortionEffectObj.SetActive(false);
        }
    }

    public override void EnableField(bool skip = false)
    {
        base.EnableField();
        if (skip)
        {
            redTransSpr.color = redTransColor;
            DistortionEffectObj.SetActive(true);
        }
        else
        {
            redTransSpr.DOKill();
            redTransSpr.DOFade(redTransColor.a, 0.5f);
            DistortionEffectObj.SetActive(true);
        }
    }
}
