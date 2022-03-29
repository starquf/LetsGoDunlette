using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_017 : RandomEncounter
{
    public SkillPiece cheatingPiece;
    private SkillPiece skill;
    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
        Image skillImg;
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "���� ���Ӽ� �귿 ���� ȹ��";
                GameManager.Instance.Gold += 10;

                if (cheatingPiece == null)
                    Debug.LogError("���Ӽ� ������ �ȵ������");
                Debug.LogWarning("����Ÿ��� ��� �־��");
                skill = Instantiate(cheatingPiece).GetComponent<SkillPiece>();
                skill.transform.position = Vector2.zero;
                skill.transform.rotation = Quaternion.Euler(0, 0, 30f);
                skillImg = skill.GetComponent<Image>();
                skillImg.color = new Color(1, 1, 1, 0);
                skill.transform.SetParent(encounterInfoHandler.transform);
                skill.transform.localScale = Vector3.one;
                skillImg.DOFade(1, 0.5f).SetDelay(1f);
                break;
            case 1:
                showText = en_End_TextList[0];
                showImg = en_End_Image[1];
                en_End_Result = "�ִ� ü���� 5%��ŭ ���ظ� �԰� ���� �귿 ���� ȹ��";
                playerHealth.GetDamage((int)(playerHealth.maxHp * 0.05f));

                SkillPiece piece = encounterInfoHandler.GetRandomSkillRewards(1)[0].GetComponent<SkillPiece>();

                skill = Instantiate(piece).GetComponent<SkillPiece>();
                skill.transform.position = Vector2.zero;
                skill.transform.rotation = Quaternion.Euler(0, 0, 30f);
                skillImg = skill.GetComponent<Image>();
                skillImg.color = new Color(1, 1, 1, 0);
                skill.transform.SetParent(encounterInfoHandler.transform);
                skill.transform.localScale = Vector3.one;
                skillImg.DOFade(1, 0.5f).SetDelay(1f);
                break;
            case 2:
                showText = en_End_TextList[0];
                showImg = en_End_Image[2];
                en_End_Result = "���� ����� 10%�� �Ұ� �ִ� ü���� 10%��ŭ ȸ��.";
                GameManager.Instance.Gold = (int)(GameManager.Instance.Gold * 0.9f);
                playerHealth.Heal((int)(playerHealth.maxHp * 0.1f));
                break;
            default:
                break;
        }
    }

    public override void Result()
    {
        BattleHandler battleHandler = GameManager.Instance.battleHandler;
        Transform unusedInventoryTrm = GameManager.Instance.inventoryHandler.transform;
        switch (choiceIdx)
        {
            case 0:
                DOTween.Sequence()
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
            case 1:
                DOTween.Sequence()
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
                OnExitEncounter?.Invoke(true);
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
