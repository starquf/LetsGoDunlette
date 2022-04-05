using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_016 : RandomEncounter
{
    RandomEncounterUIHandler encounterUIHandler = null;
    private List<int> encounterIdxList;

    public override void Init()
    {
        encounterIdxList = new List<int>();
        encounterUIHandler = encounterInfoHandler.GetComponent<RandomEncounterUIHandler>();
        for (int i = 0; i < 3; i++)
        {
            int randIdx = -1;
            while (!encounterUIHandler.CanStartEncounter(randIdx) || (encounterIdxList.Contains(randIdx) || randIdx == 15))// 15�� �ٲ�ߵ�
            {
                randIdx = Random.Range(0, encounterUIHandler.randomEncounterList.Count);
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
                en_End_Result = $"���� ��ī���Ͱ� {encounterUIHandler.randomEncounterList[encounterIdxList[0]].en_Name}�� ������";
                break;
            case 1:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                encounterUIHandler.SetRandomEncounter(encounterIdxList[1]);
                en_End_Result = $"���� ��ī���Ͱ� {encounterUIHandler.randomEncounterList[encounterIdxList[1]].en_Name}�� ������";
                break;
            case 2:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                encounterUIHandler.SetRandomEncounter(encounterIdxList[2]);
                en_End_Result = $"���� ��ī���Ͱ� {encounterUIHandler.randomEncounterList[encounterIdxList[2]].en_Name}�� ������";
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
