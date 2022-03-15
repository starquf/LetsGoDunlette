using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BookedEventInfo
{
    public int turn;
    public Action action;

    public BookedEventInfo(Action action, int turn = 0)
    {
        this.turn = turn;
        this.action = action;
    }
}

public class BattleEventHandler : MonoBehaviour
{
    public event Action onStartTurn;
    public event Action<SkillPiece> onCastPiece;
    public event Action onEndTurn;

    public event Action<SkillPiece> onNextSkill;
    private event Action<SkillPiece> nextSkill;

    private List<BookedEventInfo> eventBookInfos = new List<BookedEventInfo>();

    //public bool isWait = false;

    public void OnStartTurn()
    {
        onStartTurn?.Invoke();
    }

    public void OnCastPiece(SkillPiece piece)
    {
        onCastPiece?.Invoke(piece);
    }

    public void OnEndTurn()
    {
        BookedEvent();

        onEndTurn?.Invoke();
    }

    public void OnNextSkill(SkillPiece piece)
    {
        nextSkill?.Invoke(piece);
    }

    public void InitNextSkill()
    {
        nextSkill = null;
        nextSkill = onNextSkill;
    }

    public void BookEvent(BookedEventInfo bookEvent)
    {
        eventBookInfos.Add(bookEvent);
    }

    private void BookedEvent()
    {
        for (int i = eventBookInfos.Count - 1; i >= 0; i--)
        {
            eventBookInfos[i].turn--;
            if (eventBookInfos[i].turn <= 0)
            {
                onEndTurn += eventBookInfos[i].action;
                eventBookInfos.RemoveAt(i);
            }
        }
    }
}
