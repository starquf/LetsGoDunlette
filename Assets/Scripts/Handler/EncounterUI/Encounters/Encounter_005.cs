using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_005 : RandomEncounter
{
    private bool IsWin()
    {
        return true;
        int rand = Random.Range(0, 99);
        if(rand < 33)
        {
            return true;
        }
        return false;
    }

    public override void ResultSet(int resultIdx)
    {
        if(IsWin())
        {
            choiceIdx = 0;
            showText = en_End_TextList[0];
            showImg = en_End_Image[0];
            en_End_Result = "승리\n3개중 하나 선택해 주세요";

            RandomEncounterUIHandler randomEncounterUIHandler = encounterInfoHandler.GetComponent<RandomEncounterUIHandler>();
            Transform parent = randomEncounterUIHandler.imgButtonRowsCvs.transform;

            randomEncounterUIHandler.exitBtn.gameObject.SetActive(false);
            randomEncounterUIHandler.ShowPanel(true, randomEncounterUIHandler.imgButtonRowsCvs, 1f);

            List<GameObject> rulletPieces = encounterInfoHandler.GetRandomRewards(3);
            for (int i = 0; i < 3; i++)
            {
                int idx = i;
                GameObject item = parent.GetChild(idx).gameObject;
                Image image = item.GetComponent<Image>();
                //image.sprite = rulletPieces[idx].GetComponent<SkillPiece>().skillImg.sprite;
                image.sprite = rulletPieces[idx].transform.Find("SkillIcon").GetComponent<Image>().sprite;
                
                item.GetComponent<Button>().onClick.AddListener(() =>
                {
                    randomEncounterUIHandler.imgButtonRowsCvs.interactable = false;
                    randomEncounterUIHandler.ShowPanel(false, randomEncounterUIHandler.imgButtonRowsCvs);

                    // 설명창 띄워 주고 아래있는 함수 실행
                    GetRulletPiece(rulletPieces[idx].GetComponent<SkillPiece>());
                });
            }
        }
        else
        {
            choiceIdx = 1;
            showText = en_End_TextList[1];
            showImg = en_End_Image[1];
            en_End_Result = "패배";
        }
    }

    private void GetRulletPiece(SkillPiece rulletPiece)
    {
        BattleHandler battleHandler = GameManager.Instance.battleHandler;
        SkillRullet rullet = battleHandler.mainRullet;

        SkillPiece skill = Instantiate(rulletPiece, rullet.transform).GetComponent<SkillPiece>();
        skill.transform.position = Vector2.zero;
        skill.transform.rotation = Quaternion.Euler(0, 0, 30f);
        Image skillImg = skill.GetComponent<Image>();
        skillImg.color = new Color(1, 1, 1, 0);
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
