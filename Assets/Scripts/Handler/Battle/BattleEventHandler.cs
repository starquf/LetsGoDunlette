using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventTime //노말 이벤트들
{
    StartTurn, //턴 시작 할때
    EndOfTurn, //턴 끝날때
    BeginBattle,//배틀 시작할때
}

public enum EventTimeSkill //스킬 이벤트들
{
    WithSkill,//스킬 발동될때
    AfterSkill//스킬 발동 후
}

public enum EventTimeEnemy //스킬 이벤트들
{
    EnemyCreate, //적이 살았을때
    EnemyDie //적이죽었을때
}

[Serializable]
public abstract class EventInfo
{
    public bool actionOnEnd; //삭제될때만 실행되는가 (몇턴후 실행 이런거)
    public int turn;//0은 삭제할때까지

    public EventInfo()
    {
        this.actionOnEnd = false;
        this.turn = 101;
    }

    public EventInfo(bool actionOnEnd, int turn)
    {
        this.actionOnEnd = actionOnEnd;
        this.turn = turn;
    }

    public abstract void InvokeEvent(Action onEnd);
    public abstract void InvokeEvent(SkillPiece piece, Action onEnd);
    public abstract void InvokeEvent(EnemyHealth enemy, Action onEnd);
    public abstract bool CheckEventType(EventTime eventTime);
    public abstract bool CheckEventType(EventTimeSkill eventTime);
    public abstract bool CheckEventType(EventTimeEnemy eventTime);

}
[Serializable]

public class NormalEvent : EventInfo
{
    public Action<Action> action;
    public EventTime eventTime;

    public NormalEvent(Action<Action> action, EventTime eventTime)
    {
        this.action = action;
        this.eventTime = eventTime;
    }
    public NormalEvent(bool actionOnEnd, int turn, Action<Action> action, EventTime eventTime) : base(actionOnEnd, turn)
    {
        this.action = action;
        this.eventTime = eventTime;
    }

    public override bool CheckEventType(EventTime eventTime)
    {
        return this.eventTime == eventTime;
    }

    public override bool CheckEventType(EventTimeSkill eventTime)
    {
        return false;
    }

    public override bool CheckEventType(EventTimeEnemy eventTime)
    {
        return false;
    }

    public override void InvokeEvent(Action onEnd)
    {
        action?.Invoke(onEnd);
    }

    public override void InvokeEvent(SkillPiece piece, Action onEnd)
    {
        onEnd?.Invoke();
    }

    public override void InvokeEvent(EnemyHealth enemy, Action onEnd)
    {
        onEnd?.Invoke();
    }
}
[Serializable]
public class SkillEvent : EventInfo
{
    public EventTimeSkill eventTimeSkill;
    public Action<SkillPiece, Action> action;

    public SkillEvent(EventTimeSkill eventTimeSkill, Action<SkillPiece, Action> action)
    {
        this.eventTimeSkill = eventTimeSkill;
        this.action = action;
    }

    public SkillEvent(bool actionOnEnd, int turn, EventTimeSkill eventTimeSkill, Action<SkillPiece, Action> action) : base(actionOnEnd, turn)
    {
        this.eventTimeSkill = eventTimeSkill;
        this.action = action;
    }

    public override bool CheckEventType(EventTime eventTime)
    {
        return false;
    }

    public override bool CheckEventType(EventTimeSkill eventTime)
    {
        return eventTimeSkill == eventTime;
    }

    public override bool CheckEventType(EventTimeEnemy eventTime)
    {
        return false;
    }

    public override void InvokeEvent(SkillPiece piece, Action onEnd)
    {
        action?.Invoke(piece, onEnd);
    }

    public override void InvokeEvent(Action onEnd)
    {
        onEnd?.Invoke();
    }

    public override void InvokeEvent(EnemyHealth enemy, Action onEnd)
    {
        onEnd?.Invoke();
    }
}
[Serializable]
public class EnemyEvent : EventInfo
{
    public EventTimeEnemy eventTimeEnemy;
    public Action<EnemyHealth, Action> action;

    public EnemyEvent(EventTimeEnemy eventTimeEnemy, Action<EnemyHealth, Action> action)
    {
        this.eventTimeEnemy = eventTimeEnemy;
        this.action = action;
    }

    public EnemyEvent(bool actionOnEnd, int turn, EventTimeEnemy eventTimeEnemy, Action<EnemyHealth, Action> action) : base(actionOnEnd, turn)
    {
        this.eventTimeEnemy = eventTimeEnemy;
        this.action = action;
    }

    public override bool CheckEventType(EventTime eventTime)
    {
        return false;
    }

    public override bool CheckEventType(EventTimeSkill eventTime)
    {
        return false;
    }

    public override bool CheckEventType(EventTimeEnemy eventTime)
    {
        return eventTimeEnemy == eventTime;
    }

    public override void InvokeEvent(SkillPiece piece, Action onEnd)
    {
        onEnd?.Invoke();
    }

    public override void InvokeEvent(EnemyHealth enemy, Action onEnd)
    {
        action?.Invoke(enemy, onEnd);

    }

    public override void InvokeEvent(Action onEnd)
    {
        onEnd?.Invoke();
    }
}


public class BattleEventHandler : MonoBehaviour
{
    public List<EventInfo> eventInfoList = new List<EventInfo>();

    public void BookEvent(EventInfo eventInfo)
    {
        eventInfoList.Add(eventInfo);
    }

    public IEnumerator ActionEvent(EventTime eventTime, Action onEnd = null)
    {
        for (int i = eventInfoList.Count - 1; i >= 0; i--)
        {
            int a = i;
            if (eventInfoList[a].CheckEventType(eventTime))
            {
                bool flag = true;
                eventInfoList[a].turn--;
                if (eventInfoList[a].actionOnEnd) //마지막만 실행되는거라면
                {
                    if (eventInfoList[a].turn <= 0) //삭제될것
                    {
                        eventInfoList[a].InvokeEvent(() => flag = false);
                        while (flag)
                        {
                            yield return null;
                        }
                        eventInfoList.RemoveAt(a);
                    }
                    continue;
                }
                else
                {

                    eventInfoList[a].InvokeEvent(() => flag = false);
                    while (flag)
                    {
                        yield return null;
                    }
                    if (eventInfoList[a].turn <= 0) //삭제될것
                    {
                        eventInfoList.RemoveAt(a);
                    }
                }
            }
        }

        onEnd?.Invoke();
    }

    public IEnumerator ActionEvent(EventTimeSkill eventTime, SkillPiece piece, Action onEnd = null)
    {
        for (int i = eventInfoList.Count - 1; i >= 0; i--)
        {
            int a = i;
            if (eventInfoList[a].CheckEventType(eventTime))
            {
                bool flag = true;
                eventInfoList[a].turn--;

                if (eventInfoList[a] == null)
                {
                    eventInfoList.RemoveAt(a);
                    continue;
                }

                if (eventInfoList[a].actionOnEnd) //마지막만 실행되는거라면
                {
                    if (eventInfoList[a].turn <= 0) //삭제될것
                    {
                        eventInfoList[a].InvokeEvent(piece, () => flag = false);
                        while (flag)
                        {
                            yield return null;
                        }
                        eventInfoList.RemoveAt(a);
                    }
                    continue;
                }
                else
                {
                    eventInfoList[a].InvokeEvent(piece, () => flag = false);
                    while (flag)
                    {
                        yield return null;
                    }
                    if (eventInfoList[a].turn <= 0) //삭제될것
                    {
                        eventInfoList.RemoveAt(a);
                    }
                }
            }
        }

        onEnd?.Invoke();



        yield break;
    }

    public IEnumerator ActionEvent(EventTimeEnemy eventTime, EnemyHealth enemy, Action onEnd = null)
    {
        for (int i = eventInfoList.Count - 1; i >= 0; i--)
        {
            int a = i;
            if (eventInfoList[a].CheckEventType(eventTime))
            {
                bool flag = true;
                eventInfoList[a].turn--;
                if (eventInfoList[a].actionOnEnd) //마지막만 실행되는거라면
                {
                    if (eventInfoList[a].turn <= 0) //삭제될것
                    {
                        eventInfoList[a].InvokeEvent(enemy, () => flag = false);
                        eventInfoList.RemoveAt(a);
                    }
                    continue;
                }
                else
                {

                    eventInfoList[a].InvokeEvent(enemy, () => flag = false);
                    if (eventInfoList[a].turn <= 0) //삭제될것
                    {
                        eventInfoList.RemoveAt(a);
                    }
                }
            }
        }

        onEnd?.Invoke();
        yield return null;
    }

    public void StartActionEvent(EventTime eventTime, Action onEnd = null)
    {
        StartCoroutine(ActionEvent(eventTime, onEnd));
    }

    public void StartActionEvent(EventTimeSkill eventTime, SkillPiece piece, Action onEnd = null)
    {
        StartCoroutine(ActionEvent(eventTime, piece, onEnd));
    }

    public void StartActionEvent(EventTimeEnemy eventTime, EnemyHealth enemy, Action onEnd = null)
    {
        StartCoroutine(ActionEvent(eventTime, enemy, onEnd));
    }

    public void RemoveEventInfo(EventInfo info)
    {
        if (info == null) return;

        eventInfoList.Remove(info);
    }

    public void ResetAllEvents()
    {
        eventInfoList.Clear();
    }
}
