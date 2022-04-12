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

        GetComponent<EnemyHealth>().onEnemyDie = () => battleHandler.battleEvent.RemoveEventInfo(enemyEventInfo);

        enemyEvent = (enemy, action) =>
        {
            if (enemy.GetComponent<EnemyHealth>().enemyType == EnemyType.GAR)
            {
                Action<Action> eventAction = action =>
                {
                    if (GetComponent<EnemyHealth>().curHp <= 0)
                    {
                        action?.Invoke();
                        return;
                    }

                    EnemyIndicator indi = GetComponent<EnemyIndicator>();
                    indi.ShowText("스킬 강화", ()=> battleHandler.castUIHandler.ShowCasting(pieceInfo[0], () =>
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

                EventInfo enemyEventInfo = new NormalEvent(true,0,eventAction,EventTime.EndOfTurn);
                battleHandler.battleEvent.BookEvent(enemyEventInfo);
            }

            if (enemy.GetComponent<EnemyHealth>().enemyType == EnemyType.DNAM)
            {
                Action<Action> eventAction = action =>
                {
                    if (GetComponent<EnemyHealth>().curHp <= 0)
                    {
                        action?.Invoke();
                        return;
                    }

                    EnemyIndicator indi = GetComponent<EnemyIndicator>();
                    indi.ShowText("보호막", () => battleHandler.castUIHandler.ShowCasting(pieceInfo[1], () =>
                    {
                        Anim_M_Recover effect = PoolManager.GetItem<Anim_M_Recover>();
                        effect.transform.position = gameObject.transform.position;

                        GetComponent<EnemyHealth>().AddShield(300);

                        effect.Play(() =>
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
