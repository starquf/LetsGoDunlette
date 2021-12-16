using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SubSkillRullet : Rullet
{
    public Image subBorderImg;

    protected override void Start()
    {
        multiply = -1f;

        base.Start();
    }

    protected override void CastDefault()
    {
        result = null;
    }
}
