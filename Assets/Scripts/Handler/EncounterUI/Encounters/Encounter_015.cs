using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_015 : RandomEncounter
{
    private Scroll scroll = null;
    private BattleScrollHandler battleScrollHandler = null;
    public override void Init()
    {
        base.Init();
        battleScrollHandler = bh.GetComponent<BattleScrollHandler>();
        int scrollCount = 0;
        for (int i = 0; i < battleScrollHandler.slots.Count; i++)
        {
            ScrollSlot scrollSlot = battleScrollHandler.slots[i];
            if (scrollSlot.scroll != null)
            {
                scrollCount++;
            }
        }
        if (scrollCount < 2)
        {
            RandomEncounterUIHandler randomEncounterUIHandler = encounterInfoHandler.GetComponent<RandomEncounterUIHandler>();
            randomEncounterUIHandler.encounterChoiceTxtList[0].transform.parent.GetComponent<Button>().onClick.RemoveAllListeners();
            randomEncounterUIHandler.encounterChoiceTxtList[0].transform.parent.GetComponent<Button>().onClick.AddListener(() =>
            {
                randomEncounterUIHandler.encounterChoiceTxtList[0].transform.parent.GetComponent<Button>().interactable = false;
            });
        }
    }

    public SkillPiece GetRamdomSkill()
    {
        InventoryHandler inventoryHandler = GameManager.Instance.inventoryHandler;

        Inventory inven = inventoryHandler.GetPlayerInventory();

        int randIdx = Random.Range(0, inven.skills.Count);
        return inven.skills[randIdx];
    }

    public override void ResultSet(int resultIdx)
    {
        InventoryHandler inventoryHandler = GameManager.Instance.inventoryHandler;
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "°¡Áö°í ÀÖ´Â ·£´ý ½ºÅ©·Ñ 2°³¸¦ ÀÒ°í, ·£´ý À¯¹° ÇÏ³ª È¹µæ";

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

                Debug.LogWarning("À¯¹° ¹Ì±¸Çö");
                break;
            case 1:
                showText = en_End_TextList[0];
                showImg = en_End_Image[1];
                en_End_Result = "·£´ý ·ê·¿ Á¶°¢ ÇÑ°³¸¦ ÀÒ°í, ÇöÀç µ·À» 2¹è·Î ºÒ¸°´Ù.";
                SkillPiece sp = GetRamdomSkill();
                inventoryHandler.GetSkillFromInventoryOrGraveyard(sp);
                sp.transform.rotation = Quaternion.Euler(0, 0, 30);
                DOTween.Sequence()
                .Append(sp.transform.DOMove(Vector2.zero, 0.5f))
                .Append(sp.GetComponent<Image>().DOFade(0, 0.5f))
                .Join(sp.skillIconImg.DOFade(0, 0.5f))
                .OnComplete(() =>
                {
                    Destroy(sp);
                    GameManager.Instance.Gold *= 2;
                });
                break;
            case 2:
                showText = en_End_TextList[0];
                showImg = en_End_Image[2];
                en_End_Result = "°ñµå¸¦ ÀÒ°í ·£´ý ½ºÅ©·Ñ 1°³¸¦ È¹µæ";
                GameManager.Instance.Gold -= 10;

                MakeScroll(encounterInfoHandler.GetRandomScrollRewards(1)[0].scrollType, out scroll);
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
                OnExitEncounter?.Invoke(true);
                break;
            case 1:
                OnExitEncounter?.Invoke(true);
                break;
            case 2:
                BattleScrollHandler battleScrollHandler = bh.GetComponent<BattleScrollHandler>();
                RandomEncounterUIHandler randomEncounterUIHandler = encounterInfoHandler.GetComponent<RandomEncounterUIHandler>();
                scroll.GetComponent<Image>().DOFade(1, 0.5f).SetDelay(1f);

                battleScrollHandler.GetScroll(scroll, () =>
                {
                    OnExitEncounter?.Invoke(true);
                    randomEncounterUIHandler.exitBtn.gameObject.SetActive(true);
                }, true);
                break;
            default:
                break;
        }
        choiceIdx = -1;
    }
}
