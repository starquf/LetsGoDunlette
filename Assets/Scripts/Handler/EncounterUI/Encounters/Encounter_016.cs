using System.Collections.Generic;
using UnityEngine;

public class Encounter_016 : RandomEncounter
{
    private RandomEncounterUIHandler encounterUIHandler = null;
    private List<int> encounterIdxList;

    public override void Init()
    {
        base.Init();
        encounterIdxList = new List<int>();
        encounterUIHandler = encounterInfoHandler.GetComponent<RandomEncounterUIHandler>();
        for (int i = 0; i < 3; i++)
        {
            int randIdx = -1;
            while ((!encounterUIHandler.CanStartEncounter(randIdx)) || encounterIdxList.Contains(randIdx) || randIdx == 15)// 15로 바꿔야됨
            {
                List<int> curStageEncounterIdxList = encounterUIHandler.stage[GameManager.Instance.StageIdx].RandomEncounterIdx;
                int rand = Random.Range(0, curStageEncounterIdxList.Count + encounterUIHandler.stagePublicEncounterIdxList.Count);
                randIdx = rand < curStageEncounterIdxList.Count
                    ? curStageEncounterIdxList[Random.Range(0, curStageEncounterIdxList.Count)]
                    : encounterUIHandler.stagePublicEncounterIdxList[Random.Range(0, encounterUIHandler.stagePublicEncounterIdxList.Count)];
            }

            encounterIdxList.Add(randIdx);

            encounterUIHandler.encounterChoiceTxtList[i].text = encounterUIHandler.randomEncounterList[randIdx].en_Name;
        }
    }

    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                encounterUIHandler.SetRandomEncounter(encounterIdxList[0]);
                en_End_Result = $"다음 인카운터가 {encounterUIHandler.randomEncounterList[encounterIdxList[0]].en_Name}로 고정됨";
                break;
            case 1:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                encounterUIHandler.SetRandomEncounter(encounterIdxList[1]);
                en_End_Result = $"다음 인카운터가 {encounterUIHandler.randomEncounterList[encounterIdxList[1]].en_Name}로 고정됨";
                break;
            case 2:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                encounterUIHandler.SetRandomEncounter(encounterIdxList[2]);
                en_End_Result = $"다음 인카운터가 {encounterUIHandler.randomEncounterList[encounterIdxList[2]].en_Name}로 고정됨";
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
            case 2:
                OnExitEncounter?.Invoke(true);
                break;
            default:
                break;
        }
        choiceIdx = -1;
    }
}
