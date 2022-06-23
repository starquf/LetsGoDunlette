using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_Test : PlayerSkill_Cooldown
{
    public override void Cast(Action onEndSkill)
    {
        base.Cast(onEndSkill);
        bh.mainRullet.PauseRullet();

        StartCoroutine(bh.battleUtil.ResetRullet(() =>
        {
            StartCoroutine(bh.CheckPanelty(onEndSkill));
        }));
    }
}
