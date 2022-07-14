using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PieceDicScrollRect : ScrollRect
{
    private DicScrollRectInfo info;
    private LobbyScrollHandler lobbyHandler;

    private bool changeDrag = false;

    protected override void Awake()
    {
        base.Awake();

        info = GetComponent<DicScrollRectInfo>();
        lobbyHandler = info.parentScroll.GetComponent<LobbyScrollHandler>();

        print(info == null);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.delta.x) < Mathf.Abs(eventData.delta.y))
        {
            changeDrag = false;

            base.OnBeginDrag(eventData);
        }
        else 
        {
            changeDrag = true;

            info.parentScroll.OnBeginDrag(eventData);
            lobbyHandler.OnBeginDrag(eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (!changeDrag)
        {
            base.OnDrag(eventData);
        }
        else 
        {
            info.parentScroll.OnDrag(eventData);
            lobbyHandler.OnDrag(eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!changeDrag)
        {
            base.OnEndDrag(eventData);
        }
        else 
        {
            info.parentScroll.OnEndDrag(eventData);
            lobbyHandler.OnEndDrag(eventData);
        }

        changeDrag = false;
    }
}
