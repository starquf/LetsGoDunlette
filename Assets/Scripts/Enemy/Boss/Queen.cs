using System;
using System.Collections.Generic;
using UnityEngine;

public class Queen : MonoBehaviour
{
    private EnemyHealth queenHealth;
    private BattleHandler bh;
    private SkillEvent skillEventInfo = null;
    private Action<SkillPiece, Action> skillEvent;

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

        skillEvent = (sp, action) =>
        {
            print(queenHealth.maxHp / (float)queenHealth.curHp);
            if (queenHealth.maxHp / (float)queenHealth.curHp >= 2.0f)
            {
                bh.battleEvent.RemoveEventInfo(skillEventInfo);

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
                    action?.Invoke();
                });

                GetComponent<EnemyIndicator>().ShowText("��ȯ");
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
