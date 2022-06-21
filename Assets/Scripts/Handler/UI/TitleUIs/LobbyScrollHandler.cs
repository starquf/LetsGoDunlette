using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyScrollHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Scrollbar scroll;

    public List<CanvasGroup> panels = new List<CanvasGroup>();
    public List<Button> panelBtns = new List<Button>();

    private float distance;
    private float[] pos;

    private bool isDrag = false;

    private int prevIdx;
    private int targetIdx;

    private Tween moveTween;

    public LobbyUIPanel lobbyPanel;

    private void Awake()
    {
        scroll = GetComponent<ScrollRect>().horizontalScrollbar;

        Init();
    }

    private void Init()
    {
        pos = new float[panels.Count];

        distance = 1f / (panels.Count - 1f);

        for (int i = 0; i < panels.Count; i++)
        {
            int a = i;

            pos[i] = pos[i] = distance * i; ;

            panelBtns[i].onClick.AddListener(() =>
            {
                targetIdx = a;

                StopOnMove();

                MoveToTarget();
            });
        }

        targetIdx = 1;
        scroll.value = pos[targetIdx];
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        moveTween.Kill();
        prevIdx = GetTargetPos();

        StopOnMove();
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;

        targetIdx = GetTargetPos();

        if (prevIdx == targetIdx)
        {
            // �� ���� ������ ��ǥ�� �ϳ� ����
            if (eventData.delta.x > 18 && prevIdx > 0)
            {
                --targetIdx;
            }

            // �� ���� ������ ��ǥ�� �ϳ� ����
            else if (eventData.delta.x < -18 && prevIdx < pos.Length - 1)
            {
                ++targetIdx;
            }
        }

        MoveToTarget();
    }

    private void StopOnMove()
    {
        SetPanels(false);
        lobbyPanel.ShowPlayerInfoCG(false, true);
    }

    private void MoveToTarget()
    {
        moveTween.Kill();
        moveTween = DOTween.To(() => scroll.value, value => scroll.value = value, pos[targetIdx], 0.35f)
            .OnComplete(() => 
            {
                lobbyPanel.ShowPlayerInfoCG(true, false);
                SetPanels(true);
            });
    }

    private int GetTargetPos()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            if (scroll.value < pos[i] + distance * 0.5f && scroll.value > pos[i] - distance * 0.5f)
            {
                return i;
            }
        }

        return 0;
    }

    private void SetPanels(bool enable)
    {
        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].interactable = enable;
            panels[i].blocksRaycasts = enable;
        }
    }
}