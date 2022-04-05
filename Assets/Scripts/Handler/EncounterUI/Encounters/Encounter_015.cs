using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_015 : RandomEncounter
{
    private SkillPiece skill;
    BattleScrollHandler battleScrollHandler= null;

    public override void Init()
    {
        battleScrollHandler = GameManager.Instance.battleHandler.GetComponent<BattleScrollHandler>();
        int scrollCount = 0;
        for (int i = 0; i < battleScrollHandler.slots.Count; i++)
        {
            ScrollSlot scrollSlot = battleScrollHandler.slots[i];
            if (scrollSlot.scroll != null)
            {
                scrollCount++;
            }
        }
        if(scrollCount<2)
        {
            RandomEncounterUIHandler randomEncounterUIHandler = encounterInfoHandler.GetComponent<RandomEncounterUIHandler>();
            randomEncounterUIHandler.encounterChoiceTxtList[0].transform.parent.GetComponent<Button>().onClick.RemoveAllListeners();
            randomEncounterUIHandler.encounterChoiceTxtList[0].transform.parent.GetComponent<Button>().onClick.AddListener(() =>
            {
                Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
                textEffect.Play("��ũ���� 2�� �̻��� �ƴմϴ�.");
                randomEncounterUIHandler.encounterChoiceTxtList[0].transform.parent.GetComponent<Button>().interactable = false;
            });
        }
    }

    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        Image skillImg;
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "������ �ִ� ���� ��ũ�� 2���� �Ұ�, ���� ���� �ϳ� ȹ��";

                List<ScrollSlot> scrollList = new List<ScrollSlot>();
                for (int i = 0; i < battleScrollHandler.slots.Count; i++)
                {
                    ScrollSlot scrollSlot = battleScrollHandler.slots[i];
                    if (scrollSlot.scroll != null)
                    {
                        scrollList.Add(scrollSlot);
                    }
                }
                for (int i = 0; i < 2; i++)
                {
                    int randIdx = Random.Range(0, scrollList.Count);
                    scrollList[randIdx].RemoveScroll();
                    scrollList.Remove(scrollList[randIdx]);
                }
                battleScrollHandler.SortScroll();

                print("���� �̱���");
                break;
            case 1:
                showText = en_End_TextList[0];
                showImg = en_End_Image[1];
                en_End_Result = "���� ���� �Ѱ��� �Ұ�, ���� ���� 2��� �Ҹ���.";
                GameManager.Instance.Gold *= 2;

                print("���� �̱���");
                break;
            case 2:
                showText = en_End_TextList[0];
                showImg = en_End_Image[2];
                en_End_Result = "��带 �Ұ� ���� ��ũ�� 1���� ȹ��";
                GameManager.Instance.Gold -= 10;

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
                OnExitEncounter?.Invoke(true);
                break;
            case 1:
                OnExitEncounter?.Invoke(true);
                break;
            case 2:
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
            default:
                break;
        }
        choiceIdx = -1;
    }
}
