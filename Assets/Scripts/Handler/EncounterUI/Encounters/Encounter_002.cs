using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter_002 : RandomEncounter
{

    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "코볼트와 전투 시작";
                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "무시했다...";
                break;
            default:
                break;
        }
    }

    public override void Result()
    {
        switch (choiceIdx)
        {
            case 0:
                BattleInfo bInfo = new BattleInfo();
                bInfo.enemyInfos = new List<EnemyType>() { EnemyType.KOBOLD };
                bInfo.rewards = encounterInfoHandler.encounterInfo.rewards;
                bInfo.isWeakEnemy = false;

                GameManager.Instance.battleHandler.StartBattle(false, bInfo);
                OnExitEncounter?.Invoke(false);
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
