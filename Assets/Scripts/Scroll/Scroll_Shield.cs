using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Shield : Scroll
{
    private BattleHandler bh;

    private void Start()
    {
        bh = GameManager.Instance.battleHandler;

        scrollType = ScrollType.Shield;
    }

    public override void Use(Action onEndUse, Action onCancelUse)
    {
        bh.player.AddShield(50);

        Anim_M_Shield shieldEffect = PoolManager.GetItem<Anim_M_Shield>();
        shieldEffect.transform.position = bh.playerImgTrans.position;

        shieldEffect.Play(() =>
        {
            onEndUse?.Invoke();
        });
    }
}
