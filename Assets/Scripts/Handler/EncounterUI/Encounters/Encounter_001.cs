using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_001 : RandomEncounter
{
    public int getGoldValue = 10;
    private SkillPiece skill;
    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "°ñµå È¹µæ";
                GameManager.Instance.Gold += getGoldValue;
                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "ÃÖ´ë Ã¼·ÂÀÇ 30% ¸¸Å­ È¸º¹";
                PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
                playerHealth.Heal((int)(playerHealth.maxHp * 0.3f));
                break;
            case 2:
                showText = en_End_TextList[2];
                showImg = en_End_Image[2];
                en_End_Result = "¹«ÀÛÀ§ ·ê·¿ Á¶°¢ È¹µæ";

                SkillPiece piece = encounterInfoHandler.GetRandomSkillRewards(1)[0].GetComponent<SkillPiece>();

                skill = Instantiate(piece).GetComponent<SkillPiece>();
                skill.transform.position = Vector2.zero;
                skill.transform.rotation = Quaternion.Euler(0, 0, 30f);
                Image skillImg = skill.GetComponent<Image>();
                skillImg.color = new Color(1, 1, 1, 0);
                skill.transform.SetParent(encounterInfoHandler.transform);
                skill.transform.localScale = Vector3.one;
                skillImg.DOFade(1, 0.5f).SetDelay(1f);
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
                Transform unusedInventoryTrm = GameManager.Instance.inventoryHandler.transform;
                DOTween.Sequence()
                .Append(skill.transform.DOMove(unusedInventoryTrm.position, 0.5f))
                .Join(skill.transform.DOScale(Vector2.one * 0.1f, 0.5f))
                .Join(skill.GetComponent<Image>().DOFade(0f, 0.5f))
                .OnComplete(() =>
                {
                    Inventory owner = bh.player.GetComponent<Inventory>();
                    GameManager.Instance.inventoryHandler.AddSkill(skill, owner);
                    skill.GetComponent<Image>().color = Color.white;

                    OnExitEncounter?.Invoke(true);
                });

                break;
            default:
                break;
        }
        choiceIdx = -1;
    }
}
