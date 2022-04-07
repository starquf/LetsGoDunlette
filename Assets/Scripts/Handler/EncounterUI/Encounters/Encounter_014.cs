using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_014 : RandomEncounter
{
    public int queenGetDamage = 50;
    public RF_Skill redfoxSkill = null;
    public override void ResultSet(int resultIdx)
    {
        BattleHandler bh = GameManager.Instance.battleHandler;
        choiceIdx = resultIdx;
        Action onBattleStart = null;
        switch (resultIdx)
        {
            case 0:
                BattleScrollHandler battleScrollHandler = GameManager.Instance.battleHandler.GetComponent<BattleScrollHandler>();
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "보스가 여왕으로 고정됨, 여왕의 체력이 50 닳은 상태로 시작";

                int idx = -1;
                for (int i = 0; i < battleScrollHandler.slots.Count; i++)
                {
                    ScrollSlot scrollSlot = battleScrollHandler.slots[i];
                    if (scrollSlot.scroll != null)
                    {
                        if(scrollSlot.scroll.scrollType == ScrollType.Heal)
                        {
                            idx = i;
                            break;
                        }
                    }
                }
                battleScrollHandler.slots[idx].RemoveScroll();
                battleScrollHandler.SortScroll();

                bh._bInfo.enemyInfos.Clear();
                bh._bInfo.enemyInfos.Add(EnemyType.QUEEN);
                GameManager.Instance.mapHandler.SetBossIcon(0);

                bh.battleEvent.onStartBattle -= onBattleStart;
                onBattleStart = () =>
                {
                    if (bh.isBoss)
                    {
                        bh.enemys[0].GetDamage(queenGetDamage);
                        bh.battleEvent.onStartBattle -= onBattleStart;
                    }
                };

                bh.battleEvent.onStartBattle += onBattleStart;
                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "보스가 여우로 고정된다. 여우의 스킬 룰렛 조각이 1개 추가 된다.";

                bh._bInfo.enemyInfos.Clear();
                bh._bInfo.enemyInfos.Add(EnemyType.REDFOX);
                GameManager.Instance.mapHandler.SetBossIcon(1);

                bh.battleEvent.onStartBattle -= onBattleStart;
                onBattleStart = () =>
                {
                    if (bh.isBoss)
                    {
                        GameManager.Instance.inventoryHandler.CreateSkill(redfoxSkill.gameObject, bh.enemys[0].GetComponent<Inventory>());
                        bh.battleEvent.onStartBattle -= onBattleStart;
                    }
                };

                bh.battleEvent.onStartBattle += onBattleStart;
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
