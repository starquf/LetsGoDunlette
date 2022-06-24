using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_019 : RandomEncounter
{
    public SkillPiece computerErrorPiece;
    private SkillPiece skill;
    //private Scroll scroll;

    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
        switch (resultIdx)
        {
            case 0:
                en_End_Result = "스크롤 1개 획득";
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];

                //MakeScroll(encounterInfoHandler.GetRandomScrollRewards(1)[0].scrollType, out scroll);

                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "전산 오류 조각을 1개 획득";

                if (computerErrorPiece == null)
                {
                    Debug.LogError("전산오류 조각이 안들어있음");
                }

                MakeSkill(computerErrorPiece, out skill);
                break;
            default:
                break;
        }
        ShowEndEncounter?.Invoke();
    }

    public override void Result()
    {
        RandomEncounterUIHandler randomEncounterUIHandler = encounterInfoHandler.GetComponent<RandomEncounterUIHandler>();
        Transform unusedInventoryTrm = GameManager.Instance.inventoryHandler.transform;
        switch (choiceIdx)
        {
            case 0:
                /*
                BattleScrollHandler battleScrollHandler = bh.GetComponent<BattleScrollHandler>();

                scroll.GetComponent<Image>().DOFade(1, 0.5f).SetDelay(1f);

                battleScrollHandler.GetScroll(scroll, () =>
                {
                    OnExitEncounter?.Invoke(true);
                    randomEncounterUIHandler.exitBtn.gameObject.SetActive(true);
                }, true);*/
                break;
            case 1:
                GetSkillInRandomEncounterAnim(skill,
                    () =>
                    {
                        OnExitEncounter?.Invoke(true);
                    });
                break;
            default:
                break;
        }
        choiceIdx = -1;
    }
}
