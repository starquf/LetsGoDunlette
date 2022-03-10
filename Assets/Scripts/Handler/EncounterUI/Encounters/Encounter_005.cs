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
                en_End_Result = "°ñµå È¹µæ";
                break;
            case 1:
                en_End_Result = "Ã¼·Â 30% È¸º¹";
                break;
            case 2:
                en_End_Result = "¹«ÀÛÀ§ ·ê·¿ Á¶°¢ È¹µæ";
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
