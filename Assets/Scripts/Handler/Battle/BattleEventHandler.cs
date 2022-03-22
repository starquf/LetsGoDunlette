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
    private BattleHandler bh;

    public event Action onStartTurn;
    private event Action<SkillPiece> onCastPiece;
    public event Action onEndTurn;

    private event Action<SkillPiece> onNextSkill;
    private event Action<SkillPiece> nextSkill;

    private List<BookedEventInfo> eventBookInfos = new List<BookedEventInfo>();

    //public bool isWait = false;

    private void Start()
    {
        bh = GameManager.Instance.battleHandler;
    }

    public void OnStartTurn()
    {
        onStartTurn?.Invoke();
    }

    public void OnCastPiece(SkillPiece piece)
    {
        onCastPiece?.Invoke(piece);
    }

    public void SetCastPiece(Action<SkillPiece> action)
    {
        if (!bh.isBattle) return;

        onCastPiece += action;
    }

    public void RemoveCastPiece(Action<SkillPiece> action)
    {
        onCastPiece -= action;
    }

    public void OnEndTurn()
    {
        BookedEvent();

        onEndTurn?.Invoke();
    }

    public void SetNextSkill(Action<SkillPiece> action)
    {
        if (!bh.isBattle) return;

        onNextSkill += action;
    }

    public void RemoveNextSkill(Action<SkillPiece> action)
    {
        onNextSkill -= action;
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
            int a = i;

            eventBookInfos[a].turn--;
            if (eventBookInfos[a].turn <= 0)
            {
                Action closer = () => { };
                closer = () => 
                {
                    eventBookInfos[a].action?.Invoke();
                    onEndTurn -= closer;
                };

                onEndTurn += closer;
                eventBookInfos.RemoveAt(a);
            }
        }
    }

    public void ResetAllEvents()
    {
        onStartTurn = null;
        onCastPiece = null;
        onEndTurn = null;
        onNextSkill = null;
        nextSkill = null;
        eventBookInfos.Clear();
    }
}
