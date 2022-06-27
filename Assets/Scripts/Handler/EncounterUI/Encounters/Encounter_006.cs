public class Encounter_006 : RandomEncounter
{
    public int gethealMaxHPPercent = 20;
    public int getGoldValue = 80;
    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
                playerHealth.Heal((int)(playerHealth.maxHp * (float)gethealMaxHPPercent / 100f));
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = $"최대 체력의 {gethealMaxHPPercent}% 만큼 회복";
                break;
            case 1:
                GameManager.Instance.Gold += getGoldValue;
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = $"{getGoldValue} 골드를 얻는다.";
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
