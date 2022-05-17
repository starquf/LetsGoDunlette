public class Encounter_004 : RandomEncounter
{

    public int lostGoldValue = 10;
    public override void Init()
    {
        base.Init();
        GameManager.Instance.Gold -= lostGoldValue;
    }

    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                GameManager.Instance.Gold += lostGoldValue;
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "Ã¼·Â 20°¨¼Ò";
                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "°ñµå ÀÒÀ½";
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
                PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
                playerHealth.GetDamage(20);
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
