using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

                if (slots[a].scroll != null || !bh.mainRullet.IsStop)
                {
                    print("스크롤 사용");
                    UseScroll(slots[a], slots[a].scroll);
                }
            });
        }

        GetScroll(PoolManager.GetItem<Scroll_Heal>());
        GetScroll(PoolManager.GetItem<Scroll_Shield>());
        GetScroll(PoolManager.GetItem<Scroll_Chaos>());
    }

    public bool HasScroll()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            ScrollSlot scrollSlot = slots[i];
            if (scrollSlot.scroll != null)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveRandomScroll()
    {
        List<ScrollSlot> scrollList = new List<ScrollSlot>();
        for (int i = 0; i < slots.Count; i++)
        {
            ScrollSlot scrollSlot = slots[i];
            if (scrollSlot.scroll != null)
            {
                scrollList.Add(scrollSlot);
            }
        }
        int randIdx = Random.Range(0, scrollList.Count);
        scrollList[randIdx].RemoveScroll();
        SortScroll();
    }

    private void ChangeScroll(int idx, Scroll scroll)
    {
        slots[idx].RemoveScroll();
        SetScroll(slots[idx], scroll);
    }

    public void GetScroll(Scroll scroll, Action OnComplete = null)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            ScrollSlot scrollSlot = slots[i];
            if(scrollSlot.scroll == null)
            {
                SetScroll(scrollSlot, scroll);
                SortScroll();
                OnComplete?.Invoke();
                return;
            }
        }

        // 여기 까지오면 모든 슬롯이 다 차있는거 선택해서 변경하는거 구현해야됨
        Debug.LogWarning("스크롤 변경 구현안됨..");
        OnComplete?.Invoke();
    }

    public void SortScroll()
    {
        List<int> emptySlotIdxList = new List<int>();
        for (int i = 0; i < slots.Count; i++)
        {
            int idx = i;
            ScrollSlot scrollSlot = slots[idx];
            if (scrollSlot.scroll == null)
            {
                emptySlotIdxList.Add(idx);
                emptySlotIdxList.Sort();
            }
            else if(emptySlotIdxList.Count > 0)
            {
                SetScroll(slots[emptySlotIdxList[0]], scrollSlot.scroll);
                slots[idx].scroll = null;

                emptySlotIdxList.RemoveAt(0);
                emptySlotIdxList.Add(idx);
            }
        }
    }

    private void UseScroll(ScrollSlot slot, Scroll scroll)
    {
        if (scroll == null) return;

        SetInteract(false);
        bh.stopHandler.SetInteract(false);

        scroll.slot = slot;

        scroll.Use(() =>
        {
            slot.RemoveScroll();
            SortScroll();
            bh.StartTurn();
        },
        () => 
        {
            bh.StartTurn();
        });
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
