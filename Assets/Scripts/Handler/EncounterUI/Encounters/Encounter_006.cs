using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter_006 : RandomEncounter
{
    public int getGoldValue = 10;

    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
                playerHealth.Heal((int)(playerHealth.maxHp * 0.3f));
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "ÃÖ´ë Ã¼·ÂÀÇ 30% ¸¸Å­ È¸º¹";
                break;
            case 1:
                GameManager.Instance.Gold += getGoldValue;
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "°ñµå È¹µæ";
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
            default:
                break;
        }
        choiceIdx = -1;
    }
}
