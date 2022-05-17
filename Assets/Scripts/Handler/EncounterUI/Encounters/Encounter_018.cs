using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_018 : RandomEncounter
{
    private List<SkillPiece> skills = new List<SkillPiece>();
    private SkillPiece skill;
    private SkillPiece skill2;
    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
        Image skill2Img;

        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "가지고 있는 조각 중 2개의 랜덤한 조각들이 각각 하나씩 더 복사된다.";


                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "원하는 룰렛 조각을 하나 선택해 복사한다.";
                break;
            default:
                break;
        }
        ShowEndEncounter?.Invoke();
    }

    public override void Result()
    {

        RandomEncounterUIHandler randomEncounterUIHandler = encounterInfoHandler.GetComponent<RandomEncounterUIHandler>();
        InventoryInfoHandler invenInfoHandler = GameManager.Instance.invenInfoHandler;
        InventoryHandler invenHandler = GameManager.Instance.inventoryHandler;

        Transform unusedInventoryTrm = GameManager.Instance.inventoryHandler.transform;
        switch (choiceIdx)
        {
            case 0:

                invenInfoHandler.closeBtn.interactable = false;

                skills = new List<SkillPiece>();
                for (int i = 0; i < 2; i++)
                {
                    int idx = i;

                    Inventory inven = invenHandler.GetPlayerInventory();

                    SkillPiece sp = inven.skills[Random.Range(0, inven.skills.Count)];
                    skills.Add(sp);

                    invenHandler.GetSkillFromInventory(sp);

                    sp.transform.position = bh.player.GetComponent<Inventory>().indicator.transform.position;
                    sp.transform.SetParent(encounterInfoHandler.transform);

                    DOTween.Sequence()
                    .Append(sp.transform.DOMove(Vector2.zero + (idx == 0 ? Vector2.up : Vector2.down), 0.5f))
                    .Join(sp.transform.DOScale(Vector2.one, 0.5f))
                    .Join(sp.transform.DORotate(new Vector3(0f, 0f, 30f), 0.5f))
                    .OnComplete(() =>
                    {
                        SkillPiece sp2 = null;
                        MakeSkill(sp, out sp2);
                        sp2.transform.position = sp.transform.position;
                        Image sp2Img = sp2.GetComponent<Image>();
                        skills.Add(sp2);

                        DOTween.Sequence()
                        .Append(sp2Img.DOFade(1, 0.3f))
                        .Append(sp.transform.DOMoveX(-1f, 0.3f))
                        .Join(sp2.transform.DOMoveX(1f, 0.3f))
                        .OnComplete(() =>
                        {
                            if (idx == 1)
                            {
                                for (int i = 0; i < skills.Count; i++)
                                {
                                    int idx = i;
                                    DOTween.Sequence().SetDelay(idx * 0.1f)
                                        .OnComplete(() =>
                                        {
                                            GetSkillInRandomEncounterAnim(skills[idx],
                                                () =>
                                                {
                                                    if (idx == skills.Count - 1)
                                                    {
                                                        OnExitEncounter?.Invoke(true);
                                                        invenInfoHandler.closeBtn.interactable = true;
                                                    }
                                                }, true);
                                        });
                                }
                            }
                        });
                    });
                }
                break;
            case 1:

                invenInfoHandler.closeBtn.interactable = false;

                Transform frontPanelTrm = invenInfoHandler.transform.parent;

                //invenInfoHandler.transform.SetParent(encounterInfoHandler.transform);
                //invenInfoHandler.desPanel.transform.SetParent(encounterInfoHandler.transform);

                invenInfoHandler.ShowInventoryInfo("복사할 조각을 선택하세요", ShowInfoRange.Inventory, sp =>
                {
                    invenInfoHandler.desPanel.ShowDescription(sp);

                    invenInfoHandler.desPanel.ShowConfirmBtn(() =>
                    {
                        invenInfoHandler.desPanel.ShowPanel(false);

                        invenInfoHandler.onCloseBtn = null;
                        invenInfoHandler.CloseInventoryInfo();

                        invenHandler.GetSkillFromInventory(sp);

                        GameManager.Instance.bottomUIHandler.ShowBottomPanel(false);

                        sp.transform.position = bh.player.GetComponent<Inventory>().indicator.transform.position;
                        sp.transform.SetParent(encounterInfoHandler.transform);

                        skill = sp;
                        DOTween.Sequence()
                        .Append(skill.transform.DOMove(Vector2.zero, 0.5f))
                        .Join(skill.transform.DOScale(Vector2.one, 0.5f))
                        .Join(skill.transform.DORotate(new Vector3(0f, 0f, 30f), 0.5f))
                        .OnComplete(() =>
                        {
                            MakeSkill(sp, out skill2);
                            Image skill2Img = skill2.GetComponent<Image>();

                            DOTween.Sequence()
                            .Append(skill2Img.DOFade(1, 0.3f))
                            .Append(skill.transform.DOMoveX(-1f, 0.3f))
                            .Join(skill2.transform.DOMoveX(1f, 0.3f))
                            .OnComplete(() =>
                            {

                                GetSkillInRandomEncounterAnim(skill,
                                    () =>
                                    {
                                        GetSkillInRandomEncounterAnim(skill2,
                                            () =>
                                            {
                                                OnExitEncounter?.Invoke(true);
                                                invenInfoHandler.closeBtn.interactable = true;
                                            }, true);
                                    }, true);
                            });
                        });
                    });
                }/*, onCancelUse*/, stopTime: false);

                //DOTween.Sequence()
                //.Append(skill.transform.DOMove(unusedInventoryTrm.position, 0.5f))
                //.Join(skill.transform.DOScale(Vector2.one * 0.1f, 0.5f))
                //.Join(skill.GetComponent<Image>().DOFade(0f, 0.5f))
                //.Append(skill2.transform.DOMove(unusedInventoryTrm.position, 0.5f))
                //.Join(skill2.transform.DOScale(Vector2.one * 0.1f, 0.5f))
                //.Join(skill2.GetComponent<Image>().DOFade(0f, 0.5f))
                //.OnComplete(() =>
                //{
                //    Inventory owner = bh.player.GetComponent<Inventory>();

                //    GameManager.Instance.inventoryHandler.AddSkill(skill, owner);
                //    skill.GetComponent<Image>().color = Color.white;

                //    GameManager.Instance.inventoryHandler.AddSkill(skill2, owner);
                //    skill2.GetComponent<Image>().color = Color.white;

                //    OnExitEncounter?.Invoke(true);
                //});
                break;
            default:
                break;
        }
        choiceIdx = -1;
    }
}
