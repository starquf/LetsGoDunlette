using UnityEngine;

public class Encounter_004 : RandomEncounter
{

    public int getGoldValue = 75;
    public int lostDamage = 20;
    public int lostMinGold = 20;
    public int lostMaxGold = 50;
    private int lostGold = 0;
    public override void Init()
    {
        base.Init();
    }

    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = $"{getGoldValue} °ñµå¸¦ ¾ò´Â´Ù.\nÃ¼·ÂÀ» {lostDamage} ÀÒ´Â´Ù.";
                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                lostGold = Random.Range(lostMinGold, lostMaxGold + 1);
                en_End_Result = $"{lostGold} °ñµå¸¦ ÀÒ´Â´Ù.";
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
                GameManager.Instance.Gold += getGoldValue;
                PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
                playerHealth.GetDamage(lostDamage);
                OnExitEncounter?.Invoke(true);
                break;
            case 1:
                GameManager.Instance.Gold -= lostGold;
                OnExitEncounter?.Invoke(true);
                break;
            default:
                break;
        }
        choiceIdx = -1;
    }
}
