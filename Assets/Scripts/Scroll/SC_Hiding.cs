using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Hiding : Scroll
{
    private void Start()
    {
        scrollType = ScrollType.Hiding;
    }

    public override void Use(Action onEndUse, Action onCancelUse)
    {
        if (!bh.isBattle) return;
        if (bh.isBoss) return;
        if(bh.isElite) return;

        bh.BattleForceEnd();
        bh.CheckBattleEnd();
        bh.mainRullet.StopForceRullet();

        //추후에 보상안나오게 해야함
    }
}
