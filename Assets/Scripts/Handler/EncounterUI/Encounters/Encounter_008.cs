using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
                    Debug.LogError("³¬½Ã Á¶°¢ÀÌ ¾Èµé¾îÀÖÀ½");
                Debug.LogWarning("¹î»éÀ¸·Î ´ë½Å ³Ö¾î³ð");
                skill = Instantiate(fisingPiece).GetComponent<SkillPiece>();
                skill.transform.position = Vector2.zero;
                skill.transform.rotation = Quaternion.Euler(0, 0, 30f);
                Image skillImg = skill.GetComponent<Image>();
                skillImg.color = new Color(1, 1, 1, 0);
                skill.transform.SetParent(encounterInfoHandler.transform);
                skill.transform.localScale = Vector3.one;
                skillImg.DOFade(1, 0.5f).SetDelay(1f);

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

                Transform unusedInventoryTrm = GameManager.Instance.inventoryHandler.transform;
                DOTween.Sequence()
                .Append(skill.transform.DOMove(unusedInventoryTrm.position, 0.5f))
                .Join(skill.transform.DOScale(Vector2.one * 0.1f, 0.5f))
                .Join(skill.GetComponent<Image>().DOFade(0f, 0.5f))
                .OnComplete(() =>
                {
                    Inventory Owner = bh.player.GetComponent<Inventory>();

                    GameManager.Instance.inventoryHandler.AddSkill(skill, Owner);
                    skill.GetComponent<Image>().color = Color.white;
                    
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
