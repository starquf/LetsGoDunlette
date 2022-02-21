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

    public override void Use(Action onEndUse)
    {
        bh.mainRullet.PauseRullet();

        bh.battleUtil.ResetRullet();
        bh.battleUtil.SetTimer(0.5f, onEndUse);
    }
}
