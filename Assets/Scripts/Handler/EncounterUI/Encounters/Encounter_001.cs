using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_001 : RandomEncounter
{
    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "°ñµå È¹µæ";
                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "Ã¼·Â 30% È¸º¹";
                break;
            case 2:
                showText = en_End_TextList[2];
                showImg = en_End_Image[2];
                en_End_Result = "¹«ÀÛÀ§ ·ê·¿ Á¶°¢ È¹µæ";
                break;
            default:
                break;
        }
    }

    public override void Result()
    {
        switch (choiceIdx)
        {
            case 0:
                GameManager.Instance.Gold += 10;
                OnExitEncounter?.Invoke(true);
                break;
            case 1:
                PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
                playerHealth.Heal((int)(playerHealth.maxHp*0.2f));
                OnExitEncounter?.Invoke(true);
                break;
            case 2:
                BattleHandler battleHandler = GameManager.Instance.battleHandler;
                SkillRullet rullet = battleHandler.mainRullet;
                RulletPiece rulletPieces = encounterInfoHandler.GetRandomRewards(1)[0];

                SkillPiece skill = Instantiate(rulletPieces, rullet.transform).GetComponent<SkillPiece>();
                skill.transform.position = Vector2.zero;
                skill.transform.rotation = Quaternion.Euler(0, 0, 30f);
                Image skillImg = skill.GetComponent<Image>();
                skillImg.color = new Color(1,1,1,0);
                skill.transform.SetParent(encounterInfoHandler.transform);


                Transform unusedInventoryTrm = GameManager.Instance.inventoryHandler.transform;
                DOTween.Sequence().Append(skillImg.DOFade(1, 0.5f)).SetDelay(1f)
                .Append(skill.transform.DOMove(unusedInventoryTrm.position, 0.5f))
                .Join(skill.transform.DOScale(Vector2.one * 0.1f, 0.5f))
                .Join(skill.GetComponent<Image>().DOFade(0f, 0.5f))
                .OnComplete(() =>
                {
                    Inventory owner = battleHandler.player.GetComponent<Inventory>();
                    skill.gameObject.SetActive(false);
                    skill.owner = owner;
                    GameManager.Instance.inventoryHandler.AddSkill(skill);
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
