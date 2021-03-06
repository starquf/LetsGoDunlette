public class Encounter_001 : RandomEncounter
{
    public int getGoldValue = 90;
    private SkillPiece skill;
    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = $"{getGoldValue} ???? ???´?.";
                GameManager.Instance.Gold += getGoldValue;
                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "?ִ? ü???? 30% ??ŭ ȸ??";
                PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
                playerHealth.Heal((int)(playerHealth.maxHp * 0.3f));
                break;
            case 2:
                showText = en_End_TextList[2];
                showImg = en_End_Image[2];
                en_End_Result = "?????? ?귿 ???? ȹ??";

                SkillPiece piece = encounterInfoHandler.GetRandomSkillRewards(1)[0].GetComponent<SkillPiece>();

                MakeSkill(piece, out skill);
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
            case 2:
                GetSkillInRandomEncounterAnim(skill,
                    () =>
                    {
                        OnExitEncounter?.Invoke(true);
                    });
                //Transform unusedInventoryTrm = GameManager.Instance.inventoryHandler.transform;
                //DOTween.Sequence()
                //.Append(skill.transform.DOMove(unusedInventoryTrm.position, 0.5f))
                //.Join(skill.transform.DOScale(Vector2.one * 0.1f, 0.5f))
                //.Join(skill.GetComponent<Image>().DOFade(0f, 0.5f))
                //.OnComplete(() =>
                //{
                //    Inventory owner = bh.player.GetComponent<Inventory>();
                //    GameManager.Instance.inventoryHandler.AddSkill(skill, owner);
                //    skill.GetComponent<Image>().color = Color.white;

                //    OnExitEncounter?.Invoke(true);
                //});

                break;
            default:
                break;
        }
        choiceIdx = -1;
    }
}
