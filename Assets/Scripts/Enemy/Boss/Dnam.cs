using System;
using UnityEngine;

public class Dnam : MonoBehaviour
{
    private Action<EnemyHealth, Action> enemyEvent;
    private EnemyEvent enemyEventInfo = null;

    public GameObject dnamSkill;

    public PieceInfo[] pieceInfo;
    void Start()
    {
        GetComponent<EnemyHealth>().onInit = Sacrifice;
    }

    private void Sacrifice()
    {
        BattleHandler battleHandler = GameManager.Instance.battleHandler;

        GetComponent<EnemyHealth>().onEnemyDie = () => battleHandler.battleEvent.RemoveEventInfo(enemyEventInfo);

        enemyEvent = (enemy, action) =>
        {
            if (enemy.GetComponent<EnemyHealth>().enemyType == EnemyType.SARO) //사로가 죽으면 든암의 스킬조각이 3장 늘어난다.
            {
                Action<Action> eventAction = action =>
                {
                    if (GetComponent<EnemyHealth>().IsDie)
                    {
                        action?.Invoke();
                        return;
                    }

                    EnemyIndicator indi = GetComponent<EnemyIndicator>();
                    indi.ShowText("조각 추가", () => battleHandler.castUIHandler.ShowCasting(pieceInfo[0], () =>
                    {
                        Inventory owner = GetComponent<EnemyInventory>();

                        for (int i = 0; i < 3; i++)
                        {
                            GameManager.Instance.inventoryHandler.CreateSkill(dnamSkill, owner,gameObject.transform.position);
                        }

                        Anim_M_Recover effect = PoolManager.GetItem<Anim_M_Recover>();
                        effect.transform.position = gameObject.transform.position;

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

            if (enemy.GetComponent<EnemyHealth>().enemyType == EnemyType.GAR) //가르가 죽으면 급속 성장의 보호막이 30만큼 증가한다.
            {
                Action<Action> eventAction = action =>
                {
                    if (GetComponent<EnemyHealth>().IsDie)
                    {
                        action?.Invoke();
                        return;
                    }

                    EnemyIndicator indi = GetComponent<EnemyIndicator>();
                    indi.ShowText("스킬 강화", () => battleHandler.castUIHandler.ShowCasting(pieceInfo[1], () =>
                    {
                        foreach (SkillPiece item in GameManager.Instance.inventoryHandler.skills)
                        {
                            DM_Skill skill = item as DM_Skill;
                            if (skill != null)
                            {
                                skill.DM_Sacrifice(() =>
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

                action?.Invoke();
            }
        };

        enemyEventInfo = new EnemyEvent(EventTimeEnemy.EnemyDie, enemyEvent);
        battleHandler.battleEvent.BookEvent(enemyEventInfo);
    }
}
