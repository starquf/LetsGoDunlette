using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter_003 : RandomEncounter
{

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
                    en_End_Result = "�̹Ͱ� ����!!";
                }
                else
                {
                    choiceIdx = 1;
                    showText = en_End_TextList[1];
                    showImg = en_End_Image[1];
                    en_End_Result = "��� ȹ��";
                }
                break;
            case 1:
                en_End_Result = "����ģ��..";
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
