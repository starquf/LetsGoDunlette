using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class QN_Skill : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        var enemys = GameManager.Instance.battleHandler.enemys;

        for (int i = 0; i < enemys.Count; i++)
        {
            var health = enemys[i];

            if (health.gameObject != owner.gameObject)
            {
                print("1");

                QN_Authority(target, onCastEnd);
                return;
            }
        }

        print("2");
        QN_Night_Trip(target, onCastEnd);
    }

    private void QN_Night_Trip(LivingEntity target, Action onCastEnd = null)
    {
       GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

        // 旋 持失
        for (int i = 0; i < value; i++)
        {
            //GameManager.Instance.battleHandler.CreateEnemy(dependent);
        }

        owner.GetComponent<EnemyIndicator>().ShowText("社発");

        Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
        effect.transform.position = owner.transform.position;

        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }

    private void QN_Authority(LivingEntity target, Action onCastEnd = null)
    {
        //GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

        var enemys = GameManager.Instance.battleHandler.enemys;

        for (int i = 0; i < enemys.Count; i++)
        {
            var health = enemys[i];

            if (health.gameObject != owner.gameObject)
            {
                health.AddShield(10);
            }
        }

        Anim_M_Shield effect = PoolManager.GetItem<Anim_M_Shield>();
        effect.transform.position = owner.transform.position;

        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });

    }


}
