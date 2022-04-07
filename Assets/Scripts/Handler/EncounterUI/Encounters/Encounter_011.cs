using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Encounter_011 : RandomEncounter
{
    public int battleCntValue = 2;
    public SkillPiece mermaidBlessingPiece;
    private SkillPiece skill;
    private SkillPiece skill2;
    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
        Image skillImg, skill2Img;
        switch (resultIdx)
        {
            case 0:
                BattleScrollHandler battleScrollHandler = GameManager.Instance.battleHandler.GetComponent<BattleScrollHandler>();
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "물속성 룰렛 조각 1개 및 인어의 축복 조각 획득";

                List<ScrollSlot> scrollList = new List<ScrollSlot>();
                for (int i = 0; i < battleScrollHandler.slots.Count; i++)
                {
                    ScrollSlot scrollSlot = battleScrollHandler.slots[i];
                    if (scrollSlot.scroll != null)
                    {
                        scrollList.Add(scrollSlot);
                    }
                }
                int randIdx = Random.Range(0, scrollList.Count);
                scrollList[randIdx].RemoveScroll();
                battleScrollHandler.SortScroll();

                if (mermaidBlessingPiece == null)
                    Debug.LogError("인어의축복 조각이 안들어있음");
                skill = Instantiate(mermaidBlessingPiece).GetComponent<SkillPiece>();
                skill.transform.position = Vector2.zero+(Vector2.right*1f);
                skill.transform.rotation = Quaternion.Euler(0, 0, 30f);
                skillImg = skill.GetComponent<Image>();
                skillImg.color = new Color(1, 1, 1, 0);
                skill.transform.SetParent(encounterInfoHandler.transform);
                skill.transform.localScale = Vector3.one;


                SkillPiece piece = null;
                do
                {
                    piece = encounterInfoHandler.GetRandomSkillRewards(1)[0].GetComponent<SkillPiece>();
                }while (piece.currentType != PatternType.Spade);

                skill2 = Instantiate(piece).GetComponent<SkillPiece>();
                skill2.transform.position = Vector2.zero + (Vector2.left * 1f);
                skill2.transform.rotation = Quaternion.Euler(0, 0, 30f);
                skill2Img = skill2.GetComponent<Image>();
                skill2Img.color = new Color(1, 1, 1, 0);
                skill2.transform.SetParent(encounterInfoHandler.transform);
                skill2.transform.localScale = Vector3.one;

                skillImg.DOFade(1, 0.5f).SetDelay(1f);
                skill2Img.DOFade(1, 0.5f).SetDelay(1f);
                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "최대 체력의 12%만큼 피해를 입고 인어의 축복 조각 획득";
                playerHealth.GetDamage((int)(playerHealth.maxHp * 0.12f));



                skill = Instantiate(mermaidBlessingPiece).GetComponent<SkillPiece>();
                skill.transform.position = Vector2.zero;
                skill.transform.rotation = Quaternion.Euler(0, 0, 30f);
                skillImg = skill.GetComponent<Image>();
                skillImg.color = new Color(1, 1, 1, 0);
                skill.transform.SetParent(encounterInfoHandler.transform);
                skill.transform.localScale = Vector3.one;
                skillImg.DOFade(1, 0.5f).SetDelay(1f);
                break;
            case 2:
                showText = en_End_TextList[2];
                showImg = en_End_Image[2];
                en_End_Result = "다음 2번의 전투 시작 시, 2턴간 침묵 상태이상 적용";
                break;
            default:
                break;
        }
        ShowEndEncounter?.Invoke();
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
                PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
                BattleEventHandler battleEventHandler = GameManager.Instance.battleHandler.battleEvent;

                int battlecount = battleCntValue;

                Action onBattleStart = null;
                onBattleStart = () =>
                {
                    battlecount--;
                    playerHealth.cc.SetCC(CCType.Silence, 2);
                    if (battlecount <= 0)
                    {
                        battleEventHandler.onStartBattle -= onBattleStart;
                    }
                };

                battleEventHandler.onStartBattle += onBattleStart;

                OnExitEncounter?.Invoke(true);
                break;
            default:
                break;
        }
        choiceIdx = -1;
    }

}
