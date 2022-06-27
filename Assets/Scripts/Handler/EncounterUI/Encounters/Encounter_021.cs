public class Encounter_021 : RandomEncounter
{
    public int getMoneyValue = 10;
    public int healpercent = 20;
    public int lostHP = 10;
    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = $"최대 체력의 {healpercent}%만큼 회복";
                playerHealth.Heal((int)(playerHealth.maxHp * ((float)healpercent/100f)));
                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = $"{getMoneyValue} 골드를 얻는다.\n체력을 {lostHP} 잃는다.";
                GameManager.Instance.Gold += getMoneyValue;
                playerHealth.GetDamage(lostHP);
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
