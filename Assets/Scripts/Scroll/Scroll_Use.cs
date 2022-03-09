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

        // �÷��̾� ��ų�� �����Ѵٸ�
        if (ih.CheckPlayerOrEnemyInUnUsedInven(true))
        {
            SkillPiece piece = ih.GetRandomPlayerOrEnemySkill(true);

            bh.CastPiece(piece);
            slot.RemoveScroll();
        }
    }
}
