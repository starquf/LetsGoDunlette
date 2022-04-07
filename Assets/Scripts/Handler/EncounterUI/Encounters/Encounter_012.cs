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

    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                GameManager.Instance.Gold -= giveMoney;
                en_End_Result = "3번의 전투동안 시작할 때마다 체력 10 회복";
                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "무시했다.";
                break;
            default:
                break;
        }
        ShowEndEncounter?.Invoke();
    }

    public override void Result()
    {
        BattleHandler battleHandler = GameManager.Instance.battleHandler;
        Transform unusedInventoryTrm = GameManager.Instance.inventoryHandler.transform;
        switch (choiceIdx)
        {
            case 0:
                PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
                BattleEventHandler battleEventHandler = GameManager.Instance.battleHandler.battleEvent;

                int battlecount = battleCntValue;

                Action onBattleStart = null;
                onBattleStart = () =>
                {
                    battlecount--;
                    playerHealth.Heal(10);
                    if (battlecount <= 0)
                    {
                        battleEventHandler.onStartBattle -= onBattleStart;
                    }
                };

                battleEventHandler.onStartBattle += onBattleStart;

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
