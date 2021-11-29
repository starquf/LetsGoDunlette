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
        base.Start();

        multiply = -1f;
    }

    protected override void RulletResult()
    {
        base.RulletResult();

        if (result != null)
        {
            subBorderImg.DOColor(result.Color, 0.55f);
            subBorderImg.GetComponent<RotateBorder>().SetSpeed(true);

            GameManager.Instance.battleHandler.results.Add(result);
        }
    }

    protected override void CastDefault()
    {
        result = null;
    }
}
