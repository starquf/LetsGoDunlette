using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

public class BattleScrollHandler : MonoBehaviour
{
    public RectTransform scrollUI;
    public List<ScrollSlot> slots = new List<ScrollSlot>();

    private Sequence scrollUISequence;

    private BattleHandler bh;
    private GoldUIHandler goldhandler;

    private bool canUse = false;

    private void Awake()
    {
        goldhandler = scrollUI.GetComponentInChildren<GoldUIHandler>();
    }

    private void Start()
    {
        bh = GetComponent<BattleHandler>();

        InitSlot();
        GetScroll(PoolManager.GetItem<Scroll_Heal>());
        GetScroll(PoolManager.GetItem<Scroll_Shield>());
        GetScroll(PoolManager.GetItem<Scroll_Chaos>());
    }

    private void InitSlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            int a = i;

            slots[a].GetComponent<Button>().onClick.RemoveAllListeners();
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
    }

    public void ShowScrollUI(bool isChangeScroll = false, bool open = true, bool skip = false)
    {
        scrollUISequence.Kill();

        if(skip)
        {
            scrollUI.anchoredPosition = new Vector2(open ? 0f : -150f, scrollUI.anchoredPosition.y);
            SetInteract(isChangeScroll && open);
            goldhandler.ShowGoldText(open, true);
        }
        else
        {
            if(!open)
            {
                SetInteract(false);
            }
            scrollUISequence = DOTween.Sequence()
                .Append(scrollUI.DOAnchorPosX(open ? 0f : -150f, 0.5f))
                .OnComplete(() =>
                {
                    SetInteract(isChangeScroll && open);
                    goldhandler.ShowGoldText(open);
                });
        }
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
    public void GetScroll(Scroll scroll, Action OnComplete = null, bool playAnim = false)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            int a = i;
            ScrollSlot scrollSlot = slots[a];
            if (scrollSlot.scroll == null)
            {
                if(!playAnim)
                {
                    SetScroll(scrollSlot, scroll);
                    SortScroll();
                    OnComplete?.Invoke();
                }
                else
                {
                    ShowScrollUI();
                    SetInteract(false);
                    GetAnim(a, scroll, () => {
                        SetScroll(scrollSlot, scroll);
                        SortScroll();
                        OnComplete?.Invoke();
                    });
                }
                return;
            }
        }

        // 여기 까지오면 모든 슬롯이 다 차있는거 선택해서 변경하는거 구현해야됨
        ChangeScroll(scroll, () =>
        {
            Debug.LogWarning("스크롤 변경 구현안됨..");
            OnComplete?.Invoke();
        });
    }

    private void GetAnim(int slotIdx, Scroll scroll, Action OnComplete = null)
    {
        Image scrollImg = scroll.GetComponent<Image>();
        DOTween.Sequence().Append(scrollImg.DOFade(1, 0.5f)).SetDelay(1f)
        .Append(scroll.transform.DOMove(slots[slotIdx].transform.position, 0.5f))
        .Join(scroll.transform.DOScale(Vector2.one * 0.1f, 0.5f))
        .Join(scroll.GetComponent<Image>().DOFade(0f, 0.5f))
        .OnComplete(() =>
        {
            scroll.GetComponent<RectTransform>().sizeDelta = new Vector2(96f, 112f);
            scrollImg.color = Color.white;
            OnComplete?.Invoke();
        });
    }

    private void ChangeScroll(Scroll scroll, Action OnChanged = null)
    {
        ShowScrollUI(isChangeScroll: true);
        for (int i = 0; i < slots.Count; i++)
        {
            int a = i;
            slots[a].GetComponent<Button>().onClick.RemoveAllListeners();
            slots[a].GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!canUse) return;

                if (slots[a].scroll != null || !bh.mainRullet.IsStop)
                {
                    SetInteract(false);
                    GetAnim(a, scroll, () => {
                        ChangeScrollData(a, scroll);
                        ShowScrollUI(open: false);

                        InitSlot();
                        OnChanged?.Invoke();
                    });
                }
            });
        }
    }

    private void ChangeScrollData(int idx, Scroll scroll)
    {
        slots[idx].RemoveScroll();
        SetScroll(slots[idx], scroll);
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

        bh.SetInteract(false);

        scroll.slot = slot;
        bh.canPause = false;

        scroll.Use(() =>
        {
            bh.canPause = true;
            slot.RemoveScroll();
            SortScroll();
            bh.StartTurn();
        },
        () => 
        {
            bh.canPause = true;
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
