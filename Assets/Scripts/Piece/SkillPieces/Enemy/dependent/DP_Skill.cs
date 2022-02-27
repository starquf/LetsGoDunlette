using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DP_Skill : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        if (Random.Range(1, 100) <= value)
        {
            DP_Duty(target, onCastEnd);
        }
        else
        {
            DP_Poke(target, onCastEnd);
        }
    }

    private void DP_Duty(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "ȸ��").OnComplete(() =>
        {
            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

            var enemys = GameManager.Instance.battleHandler.enemys;

            for (int i = 0; i < enemys.Count; i++)
            {
                var health = enemys[i];

                if (health.isBoss)
                {
                    // queen ��.
                    health.Heal(5);
                    owner.GetComponent<EnemyHealth>().GetDamageIgnoreShild(10);
                    break; // ������ 1���̶�� ����
                }
            }

            Anim_M_Recover effect = PoolManager.GetItem<Anim_M_Recover>();
            effect.transform.position = owner.transform.position;
            effect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
        //���߿� �� ����Ʈ�� �������� ������   
    }

    private void DP_Poke(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "����").OnComplete(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            Anim_M_Sword hitEffect = PoolManager.GetItem<Anim_M_Sword>();
            hitEffect.transform.position = owner.transform.position;

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
            target.GetDamage(15, owner.gameObject);
        });
    }


}
