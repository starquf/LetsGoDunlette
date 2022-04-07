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
        Image skillImg, skill2Img;

        RandomEncounterUIHandler randomEncounterUIHandler = encounterInfoHandler.GetComponent<RandomEncounterUIHandler>();
        InventoryInfoHandler invenInfoHandler = GameManager.Instance.invenInfoHandler;
        InventoryHandler invenHandler = GameManager.Instance.inventoryHandler;

        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "가지고 있는 조각 중 2개의 랜덤한 조각들이 각각 하나씩 더 복사된다.";

                invenInfoHandler.closeBtn.interactable = false;
                randomEncounterUIHandler.exitBtn.gameObject.SetActive(false);

                skills = new List<SkillPiece>();
                for (int i = 0; i < 2; i++)
                {
                    int idx = i;
                    SkillPiece sp = invenHandler.unusedSkills[Random.Range(0, invenHandler.unusedSkills.Count)];
                    skills.Add(sp);

                    invenHandler.GetSkillFromInventory(sp);

                    sp.GetComponent<Image>().raycastTarget = false;
                    sp.transform.SetParent(encounterInfoHandler.transform);

                    DOTween.Sequence()
                    .Append(sp.transform.DOMove(Vector2.zero+(idx==0 ?Vector2.up:Vector2.down), 0.5f))
                    .Join(sp.transform.DOScale(Vector2.one, 0.5f))
                    .Join(sp.transform.DORotate(new Vector3(0f, 0f, 30f), 0.5f))
                    .OnComplete(() =>
                    {
                        SkillPiece sp2 = Instantiate(sp).GetComponent<SkillPiece>();
                        skills.Add(sp2);
                        sp2.transform.position = sp.transform.position;
                        sp2.transform.rotation = Quaternion.Euler(0, 0, 30f);
                        Image sp2Img = sp2.GetComponent<Image>();
                        sp2Img.color = new Color(1, 1, 1, 0);
                        sp2Img.raycastTarget = false;
                        sp2.transform.SetParent(encounterInfoHandler.transform);
                        sp2.transform.localScale = Vector3.one;

                        DOTween.Sequence()
                        .Append(sp2Img.DOFade(1, 0.3f))
                        .Append(sp.transform.DOMoveX(-1f, 0.3f))
                        .Join(sp2.transform.DOMoveX(1f, 0.3f))
                        .OnComplete(() =>
                        {
                            if(idx == 1)
                            {
                                invenInfoHandler.closeBtn.interactable = true;
                                randomEncounterUIHandler.exitBtn.gameObject.SetActive(true);
                            }
                        });
                    });
                }

                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "원하는 룰렛 조각을 하나 선택해 복사한다.";


                invenInfoHandler.closeBtn.interactable = false;
                randomEncounterUIHandler.exitBtn.gameObject.SetActive(false);

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

                        sp.transform.SetParent(encounterInfoHandler.transform);

                        skill = sp;
                        DOTween.Sequence()
                        .Append(skill.transform.DOMove(Vector2.zero, 0.5f))
                        .Join(skill.transform.DOScale(Vector2.one, 0.5f))
                        .Join(skill.transform.DORotate(new Vector3(0f,0f,30f), 0.5f))
                        .OnComplete(() =>
                        {
                            skill2 = Instantiate(sp).GetComponent<SkillPiece>();
                            skill2.transform.position = Vector2.zero;
                            skill2.transform.rotation = Quaternion.Euler(0, 0, 30f);
                            skill2Img = skill2.GetComponent<Image>();
                            skill2Img.color = new Color(1, 1, 1, 0);
                            skill2.transform.SetParent(encounterInfoHandler.transform);
                            skill2.transform.localScale = Vector3.one;

                            DOTween.Sequence()
                            .Append(skill2Img.DOFade(1, 0.3f))
                            .Append(skill.transform.DOMoveX(-1f, 0.3f))
                            .Join(skill2.transform.DOMoveX(1f, 0.3f))
                            .OnComplete(() =>
                            {
                                invenInfoHandler.closeBtn.interactable = true;
                                randomEncounterUIHandler.exitBtn.gameObject.SetActive(true);
                            });
                        });
                    });
                }/*, onCancelUse*/, stopTime: false);
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
                for (int i = 0; i < skills.Count; i++)
                {
                    int idx = i;
                    DOTween.Sequence().SetDelay(idx*0.1f)
                    .Append(skills[idx].transform.DOMove(unusedInventoryTrm.position, 0.5f))
                    .Join(skills[idx].transform.DOScale(Vector2.one * 0.1f, 0.5f))
                    .Join(skills[idx].GetComponent<Image>().DOFade(0f, 0.5f))
                    .OnComplete(() =>
                    {
                        Inventory owner = battleHandler.player.GetComponent<Inventory>();
                        skills[idx].gameObject.SetActive(false);
                        skills[idx].owner = owner;
                        GameManager.Instance.inventoryHandler.AddSkill(skills[idx]);
                        skills[idx].GetComponent<Image>().color = Color.white;

                        if(idx==skills.Count-1)
                        {
                            OnExitEncounter?.Invoke(true);
                        }
                    });
                }
                break;
            case 1:
                DOTween.Sequence()
                .Append(skill.transform.DOMove(unusedInventoryTrm.position, 0.5f))
                .Join(skill.transform.DOScale(Vector2.one * 0.1f, 0.5f))
                .Join(skill.GetComponent<Image>().DOFade(0f, 0.5f))
                .Append(skill2.transform.DOMove(unusedInventoryTrm.position, 0.5f))
                .Join(skill2.transform.DOScale(Vector2.one * 0.1f, 0.5f))
                .Join(skill2.GetComponent<Image>().DOFade(0f, 0.5f))
                .OnComplete(() =>
                {
                    Inventory owner = battleHandler.player.GetComponent<Inventory>();
                    skill.gameObject.SetActive(false);
                    skill.owner = owner;
                    GameManager.Instance.inventoryHandler.AddSkill(skill);
                    skill.GetComponent<Image>().color = Color.white;

                    skill2.gameObject.SetActive(false);
                    skill2.owner = owner;
                    GameManager.Instance.inventoryHandler.AddSkill(skill2);
                    skill2.GetComponent<Image>().color = Color.white;

                    OnExitEncounter?.Invoke(true);
                });
                break;
            default:
                break;
        }
        choiceIdx = -1;
    }
}
