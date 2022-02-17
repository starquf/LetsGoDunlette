using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Heal : Scroll
{
    private BattleHandler bh;

    private void Start()
    {
        bh = GameManager.Instance.battleHandler;

        scrollType = ScrollType.Heal;
    }

    public override void Use(Action onEndUse)
    {
        bh.player.Heal(50);

        Anim_M_Recover recoverEffect = PoolManager.GetItem<Anim_M_Recover>();
        recoverEffect.transform.position = bh.playerImgTrans.position;

        recoverEffect.Play(() =>
        {
            onEndUse?.Invoke();
        });
    }
}
