using System;
using UnityEngine;

public class Gar : MonoBehaviour
{
    private Action<EnemyHealth, Action> enemyEvent;
    private EnemyEvent enemyEventInfo = null;
    public GameObject garSkill;
    public PieceInfo[] pieceInfo;
    public void Sacrifice()
    {
        BattleHandler battleHandler = GameManager.Instance.battleHandler;

        enemyEvent = (enemy, action) =>
        {
            if (enemy.GetComponent<EnemyHealth>().enemyType == EnemyType.SARO) //��ΰ� ������ ������ ��ų������ 3�� �þ��.
            {
                Action<Action> eventAction = action =>
                {
                    if (GetComponent<EnemyHealth>().IsDie)
                    {
                        action?.Invoke();
                        return;
                    }

                    EnemyIndicator indi = GetComponent<EnemyIndicator>();
                    indi.ShowText("���� �߰�", () => battleHandler.castUIHandler.ShowCasting(pieceInfo[0], () =>
                    {
                        Inventory Owner = GetComponent<EnemyInventory>();

                        for (int i = 0; i < 3; i++)
                        {
                            GameManager.Instance.inventoryHandler.CreateSkill(garSkill, Owner, gameObject.transform.position);
                        }

                        GameManager.Instance.animHandler.GetAnim(AnimName.M_Shield).SetPosition(transform.position)
                         .SetScale(2)
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

            if (enemy.GetComponent<EnemyHealth>().enemyType == EnemyType.DNAM) //����� ������ ��ȣ���� 300 ȹ���Ѵ�.
            {
                if (GetComponent<EnemyHealth>().IsDie)
                {
                    action?.Invoke();
                    return;
                }
                Action<Action> eventAction = action =>
                {
                    EnemyIndicator indi = GetComponent<EnemyIndicator>();
                    indi.ShowText("��ȣ��", () => battleHandler.castUIHandler.ShowCasting(pieceInfo[1], () =>
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
