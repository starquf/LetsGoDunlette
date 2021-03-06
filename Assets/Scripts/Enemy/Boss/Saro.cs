using System;
using UnityEngine;

public class Saro : MonoBehaviour
{
    private Action<EnemyHealth, Action> enemyEvent;
    private EnemyEvent enemyEventInfo = null;
    public PieceInfo[] pieceInfo;
    private BattleHandler battleHandler;

    public void Start()
    {
        Sacrifice();
    }

    public void Sacrifice()
    {
        if (battleHandler == null)
        {
            battleHandler = GameManager.Instance.battleHandler;
        }

        battleHandler.battleEvent.RemoveEventInfo(enemyEventInfo);

        enemyEvent = (enemy, action) =>
        {
            print("1");
            if (enemy.GetComponent<EnemyHealth>().enemyType == EnemyType.GAR)
            {
                print("2");
                Action<Action> eventAction = action =>
                {
                    if (GetComponent<EnemyHealth>().IsDie)
                    {
                        print("3");
                        action?.Invoke();
                        return;
                    }

                    print("4");
                    EnemyIndicator indi = GetComponent<EnemyIndicator>();
                    indi.ShowText("??ų ??ȭ", () => battleHandler.castUIHandler.ShowCasting(pieceInfo[0], () =>
                    {
                        foreach (SkillPiece item in GameManager.Instance.inventoryHandler.skills)
                        {
                            SR_Skill skill = item as SR_Skill;
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

                EventInfo enemyEventInfo = new NormalEvent(true, 0, eventAction, EventTime.EndOfTurn);
                battleHandler.battleEvent.BookEvent(enemyEventInfo);
            }

            if (enemy.GetComponent<EnemyHealth>().enemyType == EnemyType.DNAM)
            {
                Action<Action> eventAction = action =>
                {
                    if (GetComponent<EnemyHealth>().IsDie)
                    {
                        action?.Invoke();
                        return;
                    }

                    EnemyIndicator indi = GetComponent<EnemyIndicator>();
                    indi.ShowText("??ȣ??", () => battleHandler.castUIHandler.ShowCasting(pieceInfo[1], () =>
                    {
                        GetComponent<EnemyHealth>().AddShield(300);
                        GameManager.Instance.animHandler.GetAnim(AnimName.M_Shield).SetPosition(transform.position)
                         .SetScale(1)
                         .Play(() =>
                         {
                             battleHandler.castUIHandler.ShowPanel(false, false);
                             indi.HideText();
                             action?.Invoke();
                         });
                    }));
                };

                EventInfo enemyEventInfo = new NormalEvent(true, 0, eventAction, EventTime.EndOfTurn);
                battleHandler.battleEvent.BookEvent(enemyEventInfo);
            }
        };

        enemyEventInfo = new EnemyEvent(EventTimeEnemy.EnemyDie, enemyEvent);
        battleHandler.battleEvent.BookEvent(enemyEventInfo);
    }
}
