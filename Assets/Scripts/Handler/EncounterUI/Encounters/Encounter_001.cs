using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter_001 : RandomEncounter
{

    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                showImg = en_End_Image[0];
                en_End_Result = "��� ȹ��";
                break;
            case 1:
                showImg = en_End_Image[1];
                en_End_Result = "ü�� 30% ȸ��";
                break;
            case 2:
                showImg = en_End_Image[2];
                en_End_Result = "������ �귿 ���� ȹ��";
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

                break;
            case 1:

                break;
            case 2:

                break;
            default:
                break;
        }
        choiceIdx = -1;
    }
}
