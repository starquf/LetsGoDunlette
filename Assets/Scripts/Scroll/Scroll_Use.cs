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

        // �÷��̾� ��ų�� �����Ѵٸ�
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
