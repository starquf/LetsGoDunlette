using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_008 : RandomEncounter
{
    public int getGoldValue = 10;
    public SkillPiece fisingPiece;
    private SkillPiece skill;
    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "³¬½Ã Á¶°¢ È×µæ";

                if (fisingPiece == null)
                {
                    Debug.LogError("³¬½Ã Á¶°¢ÀÌ ¾Èµé¾îÀÖÀ½");
                }

                Debug.LogWarning("¹î»éÀ¸·Î ´ë½Å ³Ö¾î³ð");
                MakeSkill(fisingPiece, out skill);

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
        ShowEndEncounter?.Invoke();
    }

    public override void Result()
    {
        switch (choiceIdx)
        {
            case 0:

                GetSkillInRandomEncounterAnim(skill,
                    ()=>
                    {
                        OnExitEncounter?.Invoke(true);
                    });
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
