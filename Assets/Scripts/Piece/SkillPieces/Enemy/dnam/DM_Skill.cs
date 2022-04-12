using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DM_Skill : SkillPiece
{
    private Action<SkillPiece, Action> skillEvent;
    private SkillEvent skillEventInfo = null;
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }
    public override void OnRullet()
    {
        base.OnRullet();

        BattleHandler bh = GameManager.Instance.battleHandler;
        bh.battleEvent.RemoveEventInfo(skillEventInfo);

        skillEvent = (piece, action) =>
        {
            pieceDes = string.Format("�Ʊ� �� ������ 2���� ��ȣ���� {0}�ο��Ѵ�.", Value);

            action?.Invoke();
        };

        skillEventInfo = new SkillEvent(EventTimeSkill.WithSkill, skillEvent);
        bh.battleEvent.BookEvent(skillEventInfo);
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        DM_RapidGrowth(target, onCastEnd);
    }

    private void DM_RapidGrowth(LivingEntity target, Action onCastEnd = null) //�Ʊ� �� ������ 2���� ��ȣ���� 30�ο��Ѵ�.
    {
        SetIndicator(owner.gameObject, "�Ʊ� �ູ").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            var enemys = ShuffleList(GameManager.Instance.battleHandler.enemys);

            for (int i = 0; i < enemys.Count; i++)
            {
                if (i > 2) break;

                var health = enemys[i];

                if (health.gameObject != owner.gameObject)
                {
                    health.AddShield(Value);
                }
            }

            Anim_M_Shield effect = PoolManager.GetItem<Anim_M_Shield>();
            effect.transform.position = owner.transform.position;

            effect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    public void DM_Sacrifice(Action onCastEnd = null) // ������ ������ �޼� ������ ��ȣ���� 30��ŭ �����Ѵ�.
    {
        GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

        Anim_M_Recover effect = PoolManager.GetItem<Anim_M_Recover>();
        effect.transform.position = GameManager.Instance.enemyEffectTrm.position; effect.SetScale(2);

        AddValue(30);

        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }

    private List<T> ShuffleList<T>(List<T> list)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; ++i)
        {
            random1 = Random.Range(0, list.Count);
            random2 = Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }

        return list;
    }
}
