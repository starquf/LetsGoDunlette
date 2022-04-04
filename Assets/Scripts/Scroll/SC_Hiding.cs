using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Hiding : Scroll
{
    private BattleHandler bh;

    private void Start()
    {
        bh = GameManager.Instance.battleHandler;

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

        //���Ŀ� ����ȳ����� �ؾ���
    }
}