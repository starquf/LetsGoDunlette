using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_012 : RandomEncounter
{
    public int giveMoney = 10;
    public int battleCntValue = 3;
    public override void Start()
    {
        base.Start();
    }
    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                GameManager.Instance.Gold -= giveMoney;
                en_End_Result = "3���� �������� ������ ������ ü�� 10 ȸ��";
                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "�����ߴ�.";
                break;
            default:
                break;
        }
        ShowEndEncounter?.Invoke();
    }

    public override void Result()
    {
        Transform unusedInventoryTrm = GameManager.Instance.inventoryHandler.transform;
        switch (choiceIdx)
        {
            case 0:
                PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
                BattleEventHandler battleEventHandler = bh.battleEvent;

                int battlecount = battleCntValue;

                NormalEvent eventInfo = null;

                Action<Action> onBattleStart = null;
                onBattleStart = action =>
                {
                    battlecount--;
                    playerHealth.Heal(10);
                    if (battlecount <= 0)
                    {
                        battleEventHandler.RemoveEventInfo(eventInfo);
                    }

                    action?.Invoke();
                };

                eventInfo = new NormalEvent(onBattleStart, EventTime.BeginBattle);
                bh.battleEvent.BookEvent(eventInfo);

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
