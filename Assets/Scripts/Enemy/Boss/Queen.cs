using System;
using System.Collections.Generic;
using UnityEngine;

public class Queen : MonoBehaviour
{
    public PieceInfo[] pieceInfo;
    private EnemyHealth queenHealth;
    private BattleHandler bh;
    private NormalEvent skillEventInfo = null;
    private Action<Action> skillEvent;

    public void NightTrip() //"�����ڸ� 2�� ��ȯ�Ѵ� <sprite=11>�� 5�� ���� 2 ��´�."
    {
        if (queenHealth == null)
        {
            queenHealth = GetComponent<EnemyHealth>();
        }
        if (bh == null)
        {
            bh = GameManager.Instance.battleHandler;
        }

        bh.battleEvent.RemoveEventInfo(skillEventInfo);

        skillEvent = (action) =>
        {
            if (queenHealth.maxHp / (float)queenHealth.curHp >= 2.0f)
            {
                bh.battleEvent.RemoveEventInfo(skillEventInfo);

                EnemyIndicator indi = GetComponent<EnemyIndicator>();

                indi.HideText();

                bh.castUIHandler.ShowCasting(pieceInfo[0], () =>
                {
                    bh.battleUtil.SetTimer(0.5f, () =>
                    {
                        bh.castUIHandler.ShowPanel(false, false);
                        indi.ShowText("��ȯ", () =>
                        {
                            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

                            List<EnemyType> dependents = new List<EnemyType>();

                            //Owner.GetComponent<LivingEntity>().cc.SetCC(CCType.Exhausted, 3);

                            // �� ����
                            for (int i = 0; i < 2; i++)
                            {
                                dependents.Add(EnemyType.DEPENDENT);
                            }

                            bh.CreateEnemy(dependents, () =>
                            {
                                indi.HideText();
                                action?.Invoke();
                            });
                        });
                    });
                });
            }
            else
            {
                action?.Invoke();
            }
        };

        skillEventInfo = new NormalEvent(skillEvent, EventTime.EndOfTurn);
        bh.battleEvent.BookEvent(skillEventInfo);
    }
}
