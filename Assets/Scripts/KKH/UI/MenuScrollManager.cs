using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MenuScrollManager : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    RectTransform rectTrans;
    public Transform contentsParent;
    public float contentsWidth = 1400f;
    private int contentsCount;

    private float[] contentCenters;

    private void Awake()
    {
        rectTrans = contentsParent.GetComponent<RectTransform>();
    }

    void Start()
    {
        //contentsWidth = contentsParent.GetChild(0).GetComponent<RectTransform>().rect.width;
        contentsCount = (int)(contentsParent.GetComponent<RectTransform>().rect.width / contentsWidth);
        contentCenters = new float[contentsCount];
        contentCenters[0] = 0;
        for (int i = 1; i < ((contentsCount-1)/2)+1; i++)
        {
            contentCenters[i * 2 - 1] = (contentsWidth * i);
            contentCenters[i * 2] = -(contentsWidth * i);
        }
    }

    public void SetTransfromScroll()
    {
        RectTransform rectTrans = contentsParent.GetComponent<RectTransform>();
        int idx = 0;
        float min = Mathf.Abs(contentCenters[0] - rectTrans.anchoredPosition.x);
        for (int i = 1; i < contentCenters.Length; i++)
        {
            float dist = Mathf.Abs(contentCenters[i] - rectTrans.anchoredPosition.x);
            if (min > dist)
            {
                min = dist;
                idx = i;
            }
        }
        DOTween.To(() => rectTrans.anchoredPosition, x => rectTrans.anchoredPosition = x, new Vector2(contentCenters[idx], 0), 0.5f);
        //rectTrans.DOAnchorPosX(contentCenters[idx], 1f);
        //rectTrans.anchoredPosition = new Vector2(contentCenters[idx], rectTrans.anchoredPosition.y);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Start");
        DOTween.KillAll();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("EndDrag");
        SetTransfromScroll();
    }
}
