using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_019 : RandomEncounter
{
    public SkillPiece computerErrorPiece;
    private SkillPiece skill;
    private Scroll scroll;
    public override void Start()
    {
        base.Start();
    }
    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
        Image skillImg;
        RandomEncounterUIHandler randomEncounterUIHandler = encounterInfoHandler.GetComponent<RandomEncounterUIHandler>();
        switch (resultIdx)
        {
            case 0:
                BattleScrollHandler battleScrollHandler = bh.GetComponent<BattleScrollHandler>();
                en_End_Result = "스크롤 1개 획득";
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];

                randomEncounterUIHandler.exitBtn.gameObject.SetActive(false);

                scroll = PoolManager.GetScroll(encounterInfoHandler.GetRandomScrollRewards(1)[0].scrollType);
                scroll.transform.position = Vector2.zero;
                Image scrollImg = scroll.GetComponent<Image>();
                scrollImg.color = new Color(1, 1, 1, 0);
                scroll.transform.SetParent(encounterInfoHandler.transform);
                scroll.GetComponent<RectTransform>().sizeDelta = Vector2.one * 400f;
                scroll.transform.localScale = Vector3.one;
                scrollImg.DOFade(1, 0.5f).SetDelay(1f);

                battleScrollHandler.GetScroll(scroll, () =>
                {
                    randomEncounterUIHandler.exitBtn.gameObject.SetActive(true);
                }, true);
                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "전산 오류 조각을 1개 획득";

                if (computerErrorPiece == null)
                    Debug.LogError("전산오류 조각이 안들어있음");
                skill = Instantiate(computerErrorPiece).GetComponent<SkillPiece>();
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
                DOTween.Sequence()
                .Append(skill.transform.DOMove(unusedInventoryTrm.position, 0.5f))
                .Join(skill.transform.DOScale(Vector2.one * 0.1f, 0.5f))
                .Join(skill.GetComponent<Image>().DOFade(0f, 0.5f))
                .OnComplete(() =>
                {
                    Inventory Owner = bh.player.GetComponent<Inventory>();

                    GameManager.Instance.inventoryHandler.AddSkill(skill, Owner);
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
