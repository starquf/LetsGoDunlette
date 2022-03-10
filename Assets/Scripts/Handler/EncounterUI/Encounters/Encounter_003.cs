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
                    showImg = en_End_Image[0];
                    en_End_Result = "¹Ì¹Í°ú ÀüÅõ!!";
                }
                else
                {
                    showImg = en_End_Image[1];
                    en_End_Result = "°ñµå È¹µæ";
                }
                break;
            case 1:
                en_End_Result = "µµ¸ÁÄ£´Ù..";
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
            default:
                break;
        }
        choiceIdx = -1;
    }
}
