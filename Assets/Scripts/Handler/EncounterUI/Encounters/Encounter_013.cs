using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Encounter_013 : RandomEncounter
{
    public int gethealMaxHPPercent = 50;
    public SkillPiece GetRamdomSkill()
    {
        InventoryHandler inventoryHandler = GameManager.Instance.inventoryHandler;
        List<SkillPiece> skills = new List<SkillPiece>();

        for (int i = 0; i < inventoryHandler.skills.Count; i++)
        {
            if (inventoryHandler.skills[i].isPlayerSkill)
            {
                skills.Add(inventoryHandler.skills[i]);
            }
        }

        int randIdx = Random.Range(0, skills.Count);
        return skills[randIdx];
    }

    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = $"조건 달성 시 최대 체력의 {gethealMaxHPPercent}%만큼 회복\n실패시 랜덤 룰렛 조각 삭제와 골드 감소";

                int turnCnt = 0;
                Action<Action> onEndTurn = null;
                NormalEvent eventInfo = null;
                onEndTurn = action =>
                {
                    turnCnt++;


                    bool isClear = true;
                    for (int i = 0; i < bh.enemys.Count; i++)
                    {
                        if (!bh.enemys[i].IsDie)
                        {
                            isClear = false;
                            break;
                        }
                    }

                    if (isClear)
                    {
                        playerHealth.Heal((int)(playerHealth.maxHp * (float)gethealMaxHPPercent / 100f));
                        bh.battleEvent.RemoveEventInfo(eventInfo);
                    }
                    else if (turnCnt >= 5)
                    {
                        InventoryHandler inventoryHandler = GameManager.Instance.inventoryHandler;

                        SkillPiece sp = GetRamdomSkill();
                        inventoryHandler.GetSkillFromInventoryOrGraveyard(sp);
                        DOTween.Sequence()
                        .Append(sp.transform.DOMove(Vector2.zero, 0.5f))
                        .Join(sp.transform.DORotate(new Vector3(0, 0, 30), 0.5f))
                        .Append(sp.GetComponent<Image>().DOFade(0, 0.5f))
                        .Join(sp.skillIconImg.DOFade(0, 0.5f))
                        .AppendInterval(0.01f)
                        .OnComplete(() =>
                        {
                            inventoryHandler.RemovePiece(sp);
                        });

                        bh.battleEvent.RemoveEventInfo(eventInfo);
                    }

                    action?.Invoke();
                };
                eventInfo = new NormalEvent(onEndTurn, EventTime.EndOfTurn);
                bh.battleEvent.BookEvent(eventInfo);
                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "무시했다.";

                break;
            default:
                break;
        }
        ShowEndEncounter?.Invoke();
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
