using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_009 : RandomEncounter
{
    public override void Init()
    {
        BattleScrollHandler battleScrollHandler = GameManager.Instance.battleHandler.GetComponent<BattleScrollHandler>();

        List<ScrollSlot> scrollList = new List<ScrollSlot>();
        for (int i = 0; i < battleScrollHandler.slots.Count; i++)
        {
            ScrollSlot scrollSlot = battleScrollHandler.slots[i];
            if (scrollSlot.scroll != null)
            {
                scrollList.Add(scrollSlot);
            }
        }
        int randIdx = Random.Range(0, scrollList.Count);
        GameManager.Instance.animHandler.GetAnim(AnimName.M_Bite).SetPosition(scrollList[randIdx].transform.position)
            .Play(()=>
            {
                battleScrollHandler.SortScroll();
            });
        scrollList[randIdx].RemoveScroll();
    }

    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "놀과 전투 시작";
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
                BattleInfo bInfo = new BattleInfo();
                bInfo.enemyInfos = new List<EnemyType>() { EnemyType.GNOLL };
                bInfo.isWeakEnemy = false;

                GameManager.Instance.battleHandler.StartBattle(bInfo: bInfo);
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
