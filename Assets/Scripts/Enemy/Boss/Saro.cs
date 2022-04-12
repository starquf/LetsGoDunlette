using System;
using UnityEngine;

public class Saro : MonoBehaviour
{
    private Action<EnemyHealth, Action> enemyEvent;
    private EnemyEvent enemyEventInfo = null;

    public PieceInfo[] pieceInfo;
    void Start()
    {
        Sacrifice();
    }

    private void Sacrifice()
    {
        BattleHandler battleHandler = GameManager.Instance.battleHandler;

        battleHandler.battleEvent.RemoveEventInfo(enemyEventInfo);

        enemyEvent = (enemy, action) =>
        {
            if (enemy.GetComponent<EnemyHealth>().enemyType == EnemyType.NORMAL_SLIME)
            {
                Action<Action> eventAction = action =>
                {
                    EnemyIndicator indi = GetComponent<EnemyIndicator>();
                    indi.ShowText("°­È­",()=> battleHandler.castUIHandler.ShowCasting(pieceInfo[0], () =>
                    {
                        foreach (SkillPiece item in GameManager.Instance.inventoryHandler.skills)
                        {
                            SA_Skill skill = item as SA_Skill;
                            if (skill != null)
                            {
                                skill.SR_Sacrifice(() =>
                                {
                                    battleHandler.castUIHandler.ShowPanel(false, false);
                                    indi.HideText();
                                    action?.Invoke();
                                });
                            }
                        }
                    }));
                };

                EventInfo enemyEventInfo = new NormalEvent(true,0,eventAction,EventTime.EndOfTurn);
                battleHandler.battleEvent.BookEvent(enemyEventInfo);
            }
        };

        enemyEventInfo = new EnemyEvent(EventTimeEnemy.EnemyDie, enemyEvent);
        battleHandler.battleEvent.BookEvent(enemyEventInfo);
    }
}
