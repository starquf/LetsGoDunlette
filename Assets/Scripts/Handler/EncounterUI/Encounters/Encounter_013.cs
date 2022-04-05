using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Encounter_013 : RandomEncounter
{
    public int lostGoldValue = 10;
    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
        BattleHandler bh = GameManager.Instance.battleHandler;
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "조건 달성 시 최대 체력의 50 % 만큼 회복 실패시 랜덤 룰렛 조각 삭제와 골드 감소";

                int turnCnt = 0;
                Action onEndTurn = null;
                onEndTurn = () =>
                {
                    turnCnt++;
                    if (turnCnt >= 5)
                    {
                        Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
                        textEffect.Play("위험한 내기 실패!!!");
                        InventoryHandler inventoryHandler = GameManager.Instance.inventoryHandler;
                        List<SkillPiece> skills = new List<SkillPiece>();
                        for (int i = 0; i < inventoryHandler.skills.Count; i++)
                        {
                            if(inventoryHandler.skills[i].isPlayerSkill)
                            {
                                skills.Add(inventoryHandler.skills[i]);
                            }
                        }
                        int randIdx = Random.Range(0, skills.Count);
                        SkillPiece sp = skills[randIdx];
                        inventoryHandler.GetSkillFromInventory(sp);
                        DOTween.Sequence()
                        .Append(sp.transform.DOMove(Vector2.zero, 0.5f))
                        .Append(sp.GetComponent<Image>().DOFade(0, 0.5f))
                        .Join(sp.skillImg.DOFade(0,0.5f))
                        .OnComplete(() =>
                        {
                            Destroy(sp);
                        });
                        

                        GameManager.Instance.Gold -= lostGoldValue;

                        bh.battleEvent.onEndTurn -= onEndTurn;
                    }
                };
                bh.battleEvent.onEndTurn += onEndTurn;

                bool isNextBattle = true;
                Action onBattleStart = null;
                onBattleStart = () =>
                {
                    if (!isNextBattle)
                    {
                        Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
                        textEffect.Play("위험한 내기 성공!!!");

                        playerHealth.Heal((int)(playerHealth.maxHp * 0.5f));

                        bh.battleEvent.onStartBattle -= onBattleStart;
                        bh.battleEvent.onEndTurn -= onEndTurn;
                    }
                    isNextBattle = false;
                };
                bh.battleEvent.onStartBattle += onBattleStart;


                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "무시했다.";

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
