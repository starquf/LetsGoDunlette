using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter_003 : RandomEncounter
{
    public int getGoldValue = 10;

    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                int rand = Random.Range(0, 100);
                if(rand < 50)
                {
                    showText = en_End_TextList[0];
                    showImg = en_End_Image[0];
                    en_End_Result = "¹Ì¹Í°ú ÀüÅõ!!";
                }
                else
                {
                    GameManager.Instance.Gold += getGoldValue;
                    choiceIdx = 1;
                    showText = en_End_TextList[1];
                    showImg = en_End_Image[1];
                    en_End_Result = "°ñµå È¹µæ";
                }
                break;
            case 1:
                showText = en_End_TextList[2];
                showImg = en_End_Image[2];
                en_End_Result = "µµ¸ÁÃÆ´Ù.";
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
                bInfo.enemyInfos = new List<EnemyType>() { EnemyType.MIMIC };
                bInfo.isWeakEnemy = false;
                
                GameManager.Instance.battleHandler.StartBattle(bInfo : bInfo);
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
