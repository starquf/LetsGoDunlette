using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Chaos : Scroll
{
    private BattleHandler bh;

    private void Start()
    {
        bh = GameManager.Instance.battleHandler;

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
            bh.battleUtil.SetTimer(0.5f, () => 
            {
                StartCoroutine(bh.CheckPanelty(onEndUse));
            });
        }));
    }
}
