using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter_005 : RandomEncounter
{

    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                en_End_Result = "��� ȹ��";
                break;
            case 1:
                en_End_Result = "ü�� 30% ȸ��";
                break;
            case 2:
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
