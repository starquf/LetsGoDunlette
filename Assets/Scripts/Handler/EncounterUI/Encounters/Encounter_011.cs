using System;
using UnityEngine;

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
        switch (resultIdx)
        {
            case 0:
                //BattleScrollHandler battleScrollHandler = bh.GetComponent<BattleScrollHandler>();
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "���Ӽ� �귿 ���� 1�� �� �ξ��� �ູ ���� ȹ��";

                /*
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
                */
                if (mermaidBlessingPiece == null)
                {
                    Debug.LogError("�ξ����ູ ������ �ȵ������");
                }

                MakeSkill(mermaidBlessingPiece, out skill);

                //skill = Instantiate(mermaidBlessingPiece).GetComponent<SkillPiece>();
                //skill.transform.position = Vector2.zero + (Vector2.right * 1f);
                //skill.transform.rotation = Quaternion.Euler(0, 0, 30f);
                //skillImg = skill.GetComponent<Image>();
                //skillImg.color = new Color(1, 1, 1, 0);
                //skill.transform.SetParent(encounterInfoHandler.transform);
                //skill.transform.localScale = Vector3.one;


                SkillPiece piece = null;
                do
                {
                    piece = encounterInfoHandler.GetRandomSkillRewards(1)[0].GetComponent<SkillPiece>();
                } while (piece.currentType != ElementalType.Water);

                MakeSkill(mermaidBlessingPiece, out skill2);
                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "�ִ� ü���� 12%��ŭ ���ظ� �԰� �ξ��� �ູ ���� ȹ��";
                playerHealth.GetDamage((int)(playerHealth.maxHp * 0.12f));

                MakeSkill(mermaidBlessingPiece, out skill);
                break;
            case 2:
                showText = en_End_TextList[2];
                showImg = en_End_Image[2];
                en_End_Result = "���� 2���� ���� ���� ��, 2�ϰ� ħ�� �����̻� ����";
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

                GetSkillInRandomEncounterAnim(skill,
                    () =>
                    {
                        GetSkillInRandomEncounterAnim(skill2,
                            () =>
                            {
                                OnExitEncounter?.Invoke(true);
                            });
                    });
                //DOTween.Sequence()
                //.Append(skill.transform.DOMove(unusedInventoryTrm.position, 0.5f))
                //.Join(skill.transform.DOScale(Vector2.one * 0.1f, 0.5f))
                //.Join(skill.GetComponent<Image>().DOFade(0f, 0.5f))
                //.Append(skill2.transform.DOMove(unusedInventoryTrm.position, 0.5f))
                //.Join(skill2.transform.DOScale(Vector2.one * 0.1f, 0.5f))
                //.Join(skill2.GetComponent<Image>().DOFade(0f, 0.5f))
                //.OnComplete(() =>
                //{
                //    Inventory Owner = bh.player.GetComponent<Inventory>();

                //    GameManager.Instance.inventoryHandler.AddSkill(skill, Owner);
                //    skill.GetComponent<Image>().color = Color.white;

                //    GameManager.Instance.inventoryHandler.AddSkill(skill2, Owner);
                //    skill2.GetComponent<Image>().color = Color.white;

                //    OnExitEncounter?.Invoke(true);
                //});
                break;
            case 1:

                GetSkillInRandomEncounterAnim(skill,
                    () =>
                    {
                        OnExitEncounter?.Invoke(true);
                    });
                //DOTween.Sequence()
                //.Append(skill.transform.DOMove(unusedInventoryTrm.position, 0.5f))
                //.Join(skill.transform.DOScale(Vector2.one * 0.1f, 0.5f))
                //.Join(skill.GetComponent<Image>().DOFade(0f, 0.5f))
                //.OnComplete(() =>
                //{
                //    Inventory Owner = bh.player.GetComponent<Inventory>();

                //    GameManager.Instance.inventoryHandler.AddSkill(skill, Owner);
                //    skill.GetComponent<Image>().color = Color.white;

                //    OnExitEncounter?.Invoke(true);
                //});
                //OnExitEncounter?.Invoke(true);
                break;
            case 2:
                NormalEvent eventInfo = null;
                PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
                BattleEventHandler battleEventHandler = bh.battleEvent;

                int battlecount = battleCntValue;

                Action<Action> onBattleStart = null;
                onBattleStart = action =>
                {
                    battlecount--;
                    playerHealth.cc.SetCC(CCType.Silence, 2);
                    if (battlecount <= 0)
                    {
                        battleEventHandler.RemoveEventInfo(eventInfo);
                    }

                    action?.Invoke();
                };

                eventInfo = new NormalEvent(onBattleStart, EventTime.BeginBattle);
                battleEventHandler.BookEvent(eventInfo);

                OnExitEncounter?.Invoke(true);
                break;
            default:
                break;
        }
        choiceIdx = -1;
    }

}
