using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Chaos : Scroll
{
    public override void Start()
    {
        base.Start();
        scrollType = ScrollType.Chaos;
    }

    public override void Use(Action onEndUse, Action onCancelUse)
    {
        if (bh.mainRullet.IsStop)
        {
            onCancelUse?.Invoke();
            return;
        }

        bh.mainRullet.PauseRullet();

        StartCoroutine(bh.battleUtil.ResetRullet(() => 
        {
            print("Ω√¿€2");

            StartCoroutine(bh.CheckPanelty(onEndUse));
        }));
    }
}
