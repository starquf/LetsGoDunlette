using System;
using System.Collections.Generic;
using UnityEngine;

public class Taros : MonoBehaviour
{
    private Action<SkillPiece, Action> skillEvent;
    private SkillEvent skillEventInfo = null;

    public GameObject tarosSkill;

    public PieceInfo[] pieceInfo;

    private int skillCount = 0;

    private BattleHandler bh;

    public void Patrol()
    {
        if (bh == null)
        {
            bh = GameManager.Instance.battleHandler;
        }

        bh.battleEvent.RemoveEventInfo(skillEventInfo);

        skillCount = 0;

        skillEvent = (sp, action) =>
        {
            TA_Skill skill = sp.GetComponent<TA_Skill>();

            if (skill == null) // 발동된 스킬이 타로스의 스킬이라면
            {
                action?.Invoke();
                return;
            }

            skillCount++;

            if (skillCount >= 3) // 3번이상 발동하였다면
            {
                if (GetComponent<EnemyHealth>().IsDie || bh.player.IsDie)
                {
                    action?.Invoke();
                    return;
                }

                EnemyIndicator indi = GetComponent<EnemyIndicator>();

                indi.HideText();

                indi.ShowText("조각 추가", () =>
                bh.castUIHandler.ShowCasting(pieceInfo[0], () =>
                {
                    Inventory owner = GetComponent<EnemyInventory>();

                    for (int i = 0; i < 2; i++)
                    {
                        bh.battleUtil.SetTimer(0.25f * i, () => { GameManager.Instance.inventoryHandler.CreateSkill(tarosSkill, owner, transform.position); });
                    }

                    bh.battleUtil.SetTimer(0.5f + 0.25f, () =>
                    {
                        bh.castUIHandler.ShowPanel(false, false);

                        indi.ShowText("룰렛 초기화", () =>
                        {
                            Rullet rullet = bh.mainRullet;
                            List<RulletPiece> pieces = rullet.GetPieces();

                            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

                            for (int i = 0; i < 6; i++)
                            {
                                bh.battleUtil.SetPieceToGraveyard(i);
                            }

                            indi.HideText();
                            action?.Invoke();
                        });
                    });
                }));

                skillCount = 0;
            }
            else
            {
                action?.Invoke();
            }
        };

        skillEventInfo = new SkillEvent(EventTimeSkill.AfterSkill, skillEvent);
        bh.battleEvent.BookEvent(skillEventInfo);
    }
}
