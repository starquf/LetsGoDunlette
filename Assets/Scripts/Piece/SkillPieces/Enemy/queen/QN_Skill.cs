using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class QN_Skill : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override PieceInfo ChoiceSkill()
    {
        base.ChoiceSkill();
        var enemys = GameManager.Instance.battleHandler.enemys;

        for (int i = 0; i < enemys.Count; i++)
        {
            var health = enemys[i];

            if (health.gameObject != owner.gameObject)
            {
                onCastSkill = QN_Authority;
                return pieceInfo[0];
            }
        }
        onCastSkill = QN_Night_Trip;
        return pieceInfo[1];
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void QN_Night_Trip(LivingEntity target, Action onCastEnd = null)
    {
        GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

        List<EnemyType> dependents = new List<EnemyType>();

        // 旋 持失
        for (int i = 0; i < 2; i++)
        {
            dependents.Add(EnemyType.DEPENDENT);
        }

        GameManager.Instance.battleHandler.CreateEnemy(dependents, () =>
        {
            onCastEnd?.Invoke();
        });

        owner.GetComponent<EnemyIndicator>().ShowText("社発");
    }

    private void QN_Authority(LivingEntity target, Action onCastEnd = null)
    {
        GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

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
