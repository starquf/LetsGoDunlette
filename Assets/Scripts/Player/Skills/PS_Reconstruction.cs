using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_Reconstruction : PlayerSkill_Cooldown
{
    public override void Cast(Action onEndSkill, Action onCancelSkill)
    {
        base.Cast(onEndSkill, onCancelSkill);

        onEndSkill += () =>
        {
            OnEndSkill();
        };

        bh.mainRullet.PauseRullet();

        StartCoroutine(bh.battleUtil.ResetRullet(() =>
        {
            StartCoroutine(bh.CheckPanelty(onEndSkill));
        }));
    }
}
