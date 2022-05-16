using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Heal : Scroll
{
    public override void Start()
    {
        base.Start();
        scrollType = ScrollType.Heal;
    }

    public override void Use(Action onEndUse, Action onCancelUse)
    {
        bh.player.Heal(50);

        GameManager.Instance.animHandler.GetAnim(AnimName.M_Recover).SetPosition(bh.playerImgTrans.position)
            .SetScale(1)
            .Play(() =>
        {
            onEndUse?.Invoke();
        });
    }
}
