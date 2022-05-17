using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_021 : RandomEncounter
{
    public int getMoneyValue = 10;
    public int healpercent = 20;
    public override void Start()
    {
        base.Start();
    }
    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = $"ÃÖ´ë Ã¼·ÂÀÇ {healpercent}%¸¸Å­ È¸º¹";
                playerHealth.Heal((int)(playerHealth.maxHp * (0.01f * healpercent)));
                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "°ñµå È¹µæ";
                GameManager.Instance.Gold += getMoneyValue;
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
            default:
                break;
        }
        choiceIdx = -1;
    }
}
