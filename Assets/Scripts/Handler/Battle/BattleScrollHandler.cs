using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleScrollHandler : MonoBehaviour
{
    public List<ScrollSlot> slots = new List<ScrollSlot>();

    private BattleHandler bh;

    private bool canUse = false;

    private void Start()
    {
        bh = GetComponent<BattleHandler>();

        InitSlot();
    }

    private void InitSlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            int a = i;

            slots[a].GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!canUse) return;

                if (slots[a].scroll != null)
                {
                    print("스크롤 사용");
                    UseScroll(slots[a], slots[a].scroll);
                }
            });
        }

        SetScroll(slots[0], PoolManager.GetItem<Scroll_Heal>());
        SetScroll(slots[1], PoolManager.GetItem<Scroll_Shield>());
        SetScroll(slots[2], PoolManager.GetItem<Scroll_Memorie>());
    }

    private void UseScroll(ScrollSlot slot, Scroll scroll)
    {
        SetInteract(false);
        bh.stopHandler.SetInteract(false);

        scroll.Use(() =>
        {
            bh.StartTurn();
        });

        slot.RemoveScroll();
    }

    private void SetScroll(ScrollSlot slot, Scroll scroll)
    {
        slot.SetScroll(scroll);
    }

    public void SetInteract(bool enable)
    {
        canUse = enable;
    }
}
