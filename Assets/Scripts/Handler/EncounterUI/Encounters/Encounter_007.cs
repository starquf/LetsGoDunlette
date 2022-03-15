using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_007 : RandomEncounter
{

    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "무작위 룰렛으로 교환";
                RandomEncounterUIHandler randomEncounterUIHandler = encounterInfoHandler.GetComponent<RandomEncounterUIHandler>();
                InventoryInfoHandler invenInfoHandler = GameManager.Instance.invenInfoHandler;
                InventoryHandler invenHandler = GameManager.Instance.inventoryHandler;

                invenInfoHandler.closeBtn.interactable = false;
                randomEncounterUIHandler.exitBtn.gameObject.SetActive(false);

                Transform frontPanelTrm = invenInfoHandler.transform.parent;

                //invenInfoHandler.transform.SetParent(encounterInfoHandler.transform);
                //invenInfoHandler.desPanel.transform.SetParent(encounterInfoHandler.transform);

                invenInfoHandler.ShowInventoryInfo("교체할 조각을 선택하세요", ShowInfoRange.Inventory, sp =>
                {
                    invenInfoHandler.desPanel.ShowDescription(sp);

                    invenInfoHandler.desPanel.ShowConfirmBtn(() =>
                    {
                        invenInfoHandler.desPanel.ShowPanel(false);

                        invenInfoHandler.onCloseBtn = null;
                        invenInfoHandler.CloseInventoryInfo();

                        invenHandler.GetSkillFromInventory(sp);

                        sp.transform.SetParent(encounterInfoHandler.transform);

                        DOTween.Sequence()
                        .Append(sp.transform.DOMove(randomEncounterUIHandler.encounterImg.transform.position, 0.5f))
                        .Join(sp.transform.DOScale(Vector2.one * 0.1f, 0.5f))
                        .Join(sp.GetComponent<Image>().DOFade(0f, 0.5f))
                        .OnComplete(() =>
                        {
                            Destroy(sp);

                            BattleHandler battleHandler = GameManager.Instance.battleHandler;
                            SkillPiece rulletPieces = encounterInfoHandler.GetRandomSkillRewards(1)[0].GetComponent<SkillPiece>();
                            SkillPiece skill = Instantiate(rulletPieces).GetComponent<SkillPiece>();
                            skill.transform.position = Vector2.zero;
                            skill.transform.rotation = Quaternion.Euler(0, 0, 30f);
                            Image skillImg = skill.GetComponent<Image>();
                            skillImg.color = new Color(1, 1, 1, 0);
                            skill.transform.SetParent(encounterInfoHandler.transform);
                            skill.transform.localScale = Vector3.one;


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

                                invenInfoHandler.transform.SetParent(frontPanelTrm.transform);
                                invenInfoHandler.desPanel.transform.SetParent(frontPanelTrm.transform);

                                //invenInfoHandler.transform.SetSiblingIndex(3);
                                //invenInfoHandler.desPanel.transform.SetSiblingIndex(4);
                                invenInfoHandler.closeBtn.interactable = true;
                                randomEncounterUIHandler.exitBtn.gameObject.SetActive(true);
                                //OnExitEncounter?.Invoke(true);
                            });
                        });
                    });
                }/*, onCancelUse*/);

                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "지나간다.";
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

                OnExitEncounter?.Invoke(true);
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
