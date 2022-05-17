using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_010 : RandomEncounter
{
    public SkillPiece cheatingPiece;
    private SkillPiece skill;
    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "골드와 속임수 룰렛 조각 획득";
                GameManager.Instance.Gold += 10;

                if (cheatingPiece == null)
                {
                    Debug.LogError("속임수 조각이 안들어있음");
                }

                Debug.LogWarning("아지타토로 대신 넣어놈");

                MakeSkill(cheatingPiece, out skill);
                break;
            case 1:
                showText = en_End_TextList[0];
                showImg = en_End_Image[1];
                en_End_Result = "최대 체력의 5%만큼 피해를 입고 랜덤 룰렛 조각 획득";
                playerHealth.GetDamage((int)(playerHealth.maxHp * 0.05f));

                SkillPiece piece = encounterInfoHandler.GetRandomSkillRewards(1)[0].GetComponent<SkillPiece>();

                MakeSkill(piece, out skill);
                break;
            case 2:
                showText = en_End_TextList[0];
                showImg = en_End_Image[2];
                en_End_Result = "소지 골드의 10%를 잃고 최대 체력의 10%만큼 회복.";
                GameManager.Instance.Gold = (int)(GameManager.Instance.Gold * 0.9f);
                playerHealth.Heal((int)(playerHealth.maxHp * 0.1f));
                break;
            default:
                break;
        }
        ShowEndEncounter?.Invoke();
    }

    public override void Result()
    {
        Transform unusedInventoryTrm = GameManager.Instance.inventoryHandler.transform;
        switch (choiceIdx)
        {
            case 0:
            case 1:
                GetSkillInRandomEncounterAnim(skill,
                    () =>
                    {
                        OnExitEncounter?.Invoke(true);
                    });
                break;
            case 2:
                OnExitEncounter?.Invoke(true);
                break;
            default:
                break;
        }
        choiceIdx = -1;
    }
}
