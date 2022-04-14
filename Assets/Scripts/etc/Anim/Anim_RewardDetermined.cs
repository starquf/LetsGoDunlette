using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_RewardDetermined : AnimObj
{
    private SpriteRenderer sr;

    protected override void Awake()
    {
        base.Awake();

        sr = GetComponent<SpriteRenderer>();
    }

    public void ChangeColor(Color color)
    {
        sr.color = color;
    }

    protected override void ResetAnim()
    {
        base.ResetAnim();

        sr.color = Color.white;
    }
}
