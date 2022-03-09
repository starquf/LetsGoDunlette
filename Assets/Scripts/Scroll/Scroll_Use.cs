using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Use : Scroll
{
    private BattleHandler bh;
    private InventoryHandler ih;

    private void Start()
    {
        bh = GameManager.Instance.battleHandler;
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

        bh.mainRullet.PauseRullet();

        // 플레이어 스킬이 존재한다면
        if (ih.CheckPlayerOrEnemyInUnUsedInven(true))
        {
            SkillPiece piece = ih.GetRandomPlayerOrEnemySkill(true);

            bh.CastPiece(piece);
            slot.RemoveScroll();
        }
    }
}
