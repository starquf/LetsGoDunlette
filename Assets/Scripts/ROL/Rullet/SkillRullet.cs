using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SkillRullet : Rullet
{
    public Image borderImg;

    protected override void RulletResult()
    {
        base.RulletResult();

        if (result != null)
        {
            borderImg.DOColor(result.Color, 0.55f);
            borderImg.GetComponent<RotateBorder>().SetSpeed(true);
        }

        GameManager.Instance.battleHandler.results.Add(result);
    }

    protected override void CastDefault()
    {
        result = null;
    }
}
