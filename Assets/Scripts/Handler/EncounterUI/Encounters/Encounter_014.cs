using System;
using UnityEngine;

public class Encounter_014 : RandomEncounter
{
    public int queenGetDamage = 50;
    public RF_Skill redfoxSkill = null;
    public Sprite redFoxBackgroundSpr = null;
    public Sprite queenBackgroundSpr = null;
    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        Action<Action> onBattleStart = null;
        switch (resultIdx)
        {
            case 0:
                //BattleScrollHandler battleScrollHandler = bh.GetComponent<BattleScrollHandler>();
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "보스가 여왕으로 고정됨, 여왕의 체력이 50 닳은 상태로 시작";
                /*
                int idx = -1;
                for (int i = 0; i < battleScrollHandler.slots.Count; i++)
                {
                    ScrollSlot scrollSlot = battleScrollHandler.slots[i];
                    if (scrollSlot.scroll != null)
                    {
                        if (scrollSlot.scroll.scrollType == ScrollType.Heal)
                        {
                            idx = i;
                            break;
                        }
                    }
                }
                battleScrollHandler.slots[idx].RemoveScroll();
                battleScrollHandler.SortScroll();
                */
                bh._bossInfo.enemyInfos.Clear();
                bh._bossInfo.enemyInfos.Add(EnemyType.QUEEN);
                bh._bossInfo.bg = queenBackgroundSpr;
                //GameManager.Instance.mapHandler.SetBossIcon(0);
                Debug.LogWarning("보스 맵 아이콘 변경해줘야됨");

                NormalEvent eventInfo = null;
                onBattleStart = action =>
                {
                    if (bh.isBoss)
                    {
                        bh.enemys[0].GetDamage(queenGetDamage);
                        bh.battleEvent.RemoveEventInfo(eventInfo);
                    }

                    action?.Invoke();
                };

                eventInfo = new NormalEvent(onBattleStart, EventTime.BeginBattle);
                bh.battleEvent.BookEvent(eventInfo);

                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "보스가 여우로 고정된다. 여우의 스킬 룰렛 조각이 1개 추가 된다.";

                bh._bossInfo.enemyInfos.Clear();
                bh._bossInfo.enemyInfos.Add(EnemyType.REDFOX);
                bh._bossInfo.bg = redFoxBackgroundSpr;
                //GameManager.Instance.mapHandler.SetBossIcon(0);
                Debug.LogWarning("보스 맵 아이콘 변경해줘야됨");
                NormalEvent eventInfo1 = null;
                bh.battleEvent.RemoveEventInfo(eventInfo1);
                onBattleStart = action =>
                {
                    if (bh.isBoss)
                    {
                        GameManager.Instance.inventoryHandler.CreateSkill(redfoxSkill.gameObject, bh.enemys[0].GetComponent<Inventory>());
                        bh.battleEvent.RemoveEventInfo(eventInfo1);
                    }

                    action?.Invoke();
                };

                eventInfo1 = new NormalEvent(onBattleStart, EventTime.BeginBattle);
                bh.battleEvent.BookEvent(eventInfo1);
                break;
            default:
                break;
        }
        ShowEndEncounter?.Invoke();
    }

    public override void Result()
    {
        switch (choiceIdx)
        {
            case 0:
                OnExitEncounter?.Invoke(true);
                break;
            case 1:
                OnExitEncounter?.Invoke(true);
                break;
            default:
                break;
        }
        choiceIdx = -1;
    }
}
