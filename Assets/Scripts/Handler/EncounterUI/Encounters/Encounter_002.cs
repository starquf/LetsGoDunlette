using System.Collections.Generic;
using UnityEngine;

public class Encounter_002 : RandomEncounter
{
    public Sprite ballteBg;
    public int lostGoldValue = 10;
    public override void Init()
    {
        base.Init();
        GameManager.Instance.Gold -= lostGoldValue;
    }
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
        ShowEndEncounter?.Invoke();
    }

    public override void Result()
    {
        switch (choiceIdx)
        {
            case 0:
                BattleInfo bInfo = new BattleInfo
                {
                    enemyInfos = new List<EnemyType>() { EnemyType.KOBOLD },
                    isWeakEnemy = false,
                    bg = ballteBg
                };

                bh.StartBattle(bInfo: bInfo);
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
