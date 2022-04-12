using System;
using UnityEngine;

public class Gar : MonoBehaviour
{
    private Action<EnemyHealth, Action> enemyEvent;

    private EnemyEvent enemyEventInfo = null;

    public GameObject garSkill;

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
            if (enemy.GetComponent<EnemyHealth>().enemyType == EnemyType.SARO) //사로가 죽으면 가르의 스킬조각이 3장 늘어난다.
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
                            GameManager.Instance.inventoryHandler.CreateSkill(garSkill, owner, gameObject.transform.position);
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

            if (enemy.GetComponent<EnemyHealth>().enemyType == EnemyType.DNAM) //든암이 죽으면 보호막을 300 획득한다.
            {
                if (GetComponent<EnemyHealth>().IsDie)
                {
                    action?.Invoke();
                    return;
                }
                Action<Action> eventAction = action =>
                {
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
