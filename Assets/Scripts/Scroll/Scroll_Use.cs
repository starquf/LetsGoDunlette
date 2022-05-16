using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Use : Scroll
{
    private InventoryHandler ih;
    public override void Start()
    {
        base.Start();
        ih = GameManager.Instance.inventoryHandler;

        scrollType = ScrollType.Use;
    }

    public override void Use(Action onEndUse, Action onCancelUse)
    {
        if (bh.mainRullet.IsStop)
        {
            onCancelUse?.Invoke();
            return;
        }

        // 플레이어 스킬이 존재한다면
        if (ih.CheckPlayerOrEnemySkill(true))
        {
            bh.mainRullet.PauseRullet();

            SkillPiece piece = ih.GetRandomPlayerOrEnemySkill(true);

            bh.CastPiece(piece);
            slot.RemoveScroll();
        }
        else
        {
            onCancelUse?.Invoke();
        }
    }
}
