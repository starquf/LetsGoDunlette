using System;
using UnityEngine;

public class Dnam : MonoBehaviour
{
    private Action<EnemyHealth, Action> enemyEvent;
    private EnemyEvent enemyEventInfo = null;
    public GameObject dnamSkill;
    public PieceInfo[] pieceInfo;
    private bool isUpgraded = false;

    private BattleHandler bh;
    public void Sacrifice()
    {
        if(bh == null)
        {
            bh = GameManager.Instance.battleHandler;
        }

        bh.battleEvent.RemoveEventInfo(enemyEventInfo);

        isUpgraded = false;

        enemyEvent = (enemy, action) =>
        {
            if (enemy.GetComponent<EnemyHealth>().enemyType == EnemyType.SARO) //��ΰ� ������ ����� ��ų������ 3�� �þ��.
            {
                Action<Action> eventAction = action =>
                {
                    if (GetComponent<EnemyHealth>().IsDie)
                    {
                        action?.Invoke();
                        return;
                    }

                    EnemyIndicator indi = GetComponent<EnemyIndicator>();
                    indi.ShowText("���� �߰�", () => bh.castUIHandler.ShowCasting(pieceInfo[0], () =>
                    {
                        Inventory owner = GetComponent<EnemyInventory>();

                        for (int i = 0; i < 3; i++)
                        {
                            SkillPiece piece = GameManager.Instance.inventoryHandler.CreateSkill(dnamSkill, owner, gameObject.transform.position);
                            if (isUpgraded)
                                piece.AddValue(30);
                        }

                        GameManager.Instance.animHandler.GetAnim(AnimName.M_Shield).SetPosition(transform.position)
                        .SetScale(1)
                        .Play(() =>
                        {
                            bh.castUIHandler.ShowPanel(false, false);
                            indi.HideText();
                            action?.Invoke();
                        });
                    }));
                };

                EventInfo enemyEventInfo = new NormalEvent(true, 0, eventAction, EventTime.EndOfTurn);
                bh.battleEvent.BookEvent(enemyEventInfo);
            }

            if (enemy.GetComponent<EnemyHealth>().enemyType == EnemyType.GAR) //������ ������ �޼� ������ ��ȣ���� 30��ŭ �����Ѵ�.
            {
                Action<Action> eventAction = action =>
                {
                    if (GetComponent<EnemyHealth>().IsDie)
                    {
                        action?.Invoke();
                        return;
                    }

                    isUpgraded = true;

                    EnemyIndicator indi = GetComponent<EnemyIndicator>();
                    indi.ShowText("��ų ��ȭ", () => bh.castUIHandler.ShowCasting(pieceInfo[1], () =>
                    {
                        foreach (SkillPiece item in GameManager.Instance.inventoryHandler.skills)
                        {
                            DM_Skill skill = item as DM_Skill;
                            if (skill != null)
                            {
                                skill.DM_Sacrifice(() =>
                                {
                                    bh.castUIHandler.ShowPanel(false, false);
                                    indi.HideText();
                                    action?.Invoke();
                                });
                            }
                        }
                    }));
                };

                EventInfo enemyEventInfo = new NormalEvent(true, 0, eventAction, EventTime.EndOfTurn);
                bh.battleEvent.BookEvent(enemyEventInfo);

                action?.Invoke();
            }
        };

        enemyEventInfo = new EnemyEvent(EventTimeEnemy.EnemyDie, enemyEvent);
        bh.battleEvent.BookEvent(enemyEventInfo);
    }
}
