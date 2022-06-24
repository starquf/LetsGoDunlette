using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleScrollHandler : MonoBehaviour
{
    //public RectTransform scrollUI;
    public CanvasGroup blackPanel;
    public CanvasGroup chagneScrollPopUp;
    public ItemDesUIHandler scrollDesHandler;

    public List<ScrollSlot> slots = new List<ScrollSlot>();

    private Sequence scrollUISequence;

    private BattleHandler bh;

    private bool canUse = false;

    private string useStr = "사용";


    private void Start()
    {
        bh = GetComponent<BattleHandler>();

        InitSlot();

        //GetScroll(PoolManager.GetScroll(ScrollType.Heal));
        //GetScroll(PoolManager.GetScroll(ScrollType.Shield));
        // GetScroll(PoolManager.GetScroll(ScrollType.Chaos));
    }

    private void InitSlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            int a = i;

            slots[a].GetComponent<Button>().onClick.RemoveAllListeners();
            slots[a].GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!bh.isBattle)
                {
                    return;
                }

                if (Time.timeScale < 1f)
                {
                    return;
                }

                if (!canUse)
                {
                    return;
                }

                if (slots[a].scroll != null && !bh.mainRullet.IsStop)
                {
                    Scroll scroll = slots[a].scroll;

                    scrollDesHandler.ShowDes(scroll.ScrollName, scroll.ScrollDes, scroll.scrollIcon, useStr,
                        () =>
                        {
                            Time.timeScale = 1f;
                            UseScroll(slots[a], slots[a].scroll);
                        },

                        () =>
                        {
                            Time.timeScale = 1f;
                            bh.SetInteract(true);
                        });

                    Time.timeScale = 0.25f;
                    bh.SetInteract(false);
                }
            });
        }
    }

    public void ShowChangeScrollPopUp(bool open = true, Action onComplete = null)
    {
        blackPanel.DOFade(open ? 1f : 0f, 0.5f);
        chagneScrollPopUp.DOFade(open ? 1f : 0f, 0.5f).OnComplete(() => onComplete?.Invoke());
    }

    //public void ShowScrollUI(bool isChangeScroll = false, bool open = true, bool skip = false)
    //{
    //    scrollUISequence.Kill();

    //    if (skip)
    //    {
    //        scrollUI.anchoredPosition = new Vector2(open ? 0f : -150f, scrollUI.anchoredPosition.y);
    //        SetInteract(isChangeScroll && open);
    //    }
    //    else
    //    {
    //        if (!open)
    //        {
    //            SetInteract(false);
    //        }
    //        if (isChangeScroll)
    //        {
    //            ShowChangeScrollPopUp(open);
    //        }
    //        scrollUISequence = DOTween.Sequence()
    //            .Append(scrollUI.DOAnchorPosX(open ? 0f : -150f, 0.5f))
    //            .OnComplete(() =>
    //            {
    //                SetInteract(isChangeScroll && open);
    //            });
    //    }
    //}

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

    public bool IsFullScroll()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            ScrollSlot scrollSlot = slots[i];
            if (scrollSlot.scroll == null)
            {
                return false;
            }
        }

        return true;
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
                if (!playAnim)
                {
                    SetScroll(scrollSlot, scroll);
                    SortScroll();
                    OnComplete?.Invoke();
                }
                else
                {
                    //if (GameManager.Instance.curEncounter != mapNode.SHOP)
                    //{
                    //    ShowScrollUI();
                    //}
                    SetInteract(false);
                    GetAnim(a, scroll, () =>
                    {
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
            OnComplete?.Invoke();
        });
    }

    private void GetAnim(int slotIdx, Scroll scroll, Action OnComplete = null)
    {
        Image scrollImg = scroll.GetComponent<Image>();
        DOTween.Sequence().Append(scrollImg.DOFade(1, 0.5f)).SetDelay(0.3f)
        .Append(scroll.transform.DOMove(slots[slotIdx].transform.position, 0.3f))
        .Join(scroll.transform.DOScale(Vector2.one * 0.1f, 0.3f))
        .Join(scroll.GetComponent<Image>().DOFade(0f, 0.3f))
        .OnComplete(() =>
        {
            scroll.GetComponent<RectTransform>().sizeDelta = new Vector2(96f, 112f);
            scrollImg.color = Color.white;
            OnComplete?.Invoke();
        });
    }

    private void ChangeScroll(Scroll scroll, Action OnChanged = null)
    {
        //if (GameManager.Instance.curEncounter != mapNode.SHOP)
        //{
        //    ShowScrollUI(isChangeScroll: true);
        //}
        //else
        //{
        //    ShowChangeScrollPopUp(true, () => SetInteract(true));
        //    //SetInteract(true);
        //}

        if (GameManager.Instance.curEncounter.Equals(mapNode.RANDOMENCOUNTER))
        {
            GameManager.Instance.bottomUIHandler.ShowBottomPanel(true);
            InventoryInfoHandler inventoryInfoHandler = GameManager.Instance.invenInfoHandler;
            inventoryInfoHandler.invenBtn.interactable = false;
            inventoryInfoHandler.usedInvenBtn.interactable = false;
        }
        ShowChangeScrollPopUp(true, () => SetInteract(true));
        for (int i = 0; i < slots.Count; i++)
        {
            int a = i;
            slots[a].GetComponent<Button>().onClick.RemoveAllListeners();
            slots[a].GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!canUse)
                {
                    return;
                }

                if (slots[a].scroll != null || !bh.mainRullet.IsStop)
                {
                    SetInteract(false);
                    GetAnim(a, scroll, () =>
                    {
                        ChangeScrollData(a, scroll);
                        //if (GameManager.Instance.curEncounter != mapNode.SHOP)
                        //{
                        //    ShowScrollUI(true, open: false);
                        //}

                        if (GameManager.Instance.curEncounter.Equals(mapNode.RANDOMENCOUNTER))
                        {
                            GameManager.Instance.bottomUIHandler.ShowBottomPanel(false);
                            InventoryInfoHandler inventoryInfoHandler = GameManager.Instance.invenInfoHandler;
                            inventoryInfoHandler.invenBtn.interactable = true;
                            inventoryInfoHandler.usedInvenBtn.interactable = true;
                        }
                        ShowChangeScrollPopUp(false);

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
            else if (emptySlotIdxList.Count > 0)
            {
                SetScroll(slots[emptySlotIdxList[0]], scrollSlot.scroll);
                slots[idx].scroll = null;
                slots[idx].transform.GetChild(0).gameObject.SetActive(true);

                emptySlotIdxList.RemoveAt(0);
                emptySlotIdxList.Add(idx);
            }
        }
    }

    private void UseScroll(ScrollSlot slot, Scroll scroll)
    {
        if (scroll == null)
        {
            return;
        }

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
