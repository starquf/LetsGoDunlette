using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LSW_Skill : SkillPiece
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
            LSW_Cutting(target, onCastEnd);
        }
        else
        {
            LSW_Old_Shield(target, onCastEnd);
        }
    }

    private void LSW_Cutting(LivingEntity target, Action onCastEnd = null)
    {
        GameManager.Instance.cameraHandler.ShakeCamera(2f, 0.2f);

        target.GetDamage(30, owner.gameObject);

        Anim_M_Sword hitEffect = PoolManager.GetItem<Anim_M_Sword>();
        hitEffect.transform.position = owner.transform.position;

        hitEffect.Play(() =>
        {
            onCastEnd?.Invoke();
        });

        owner.GetComponent<EnemyHealth>().Heal(30);
    }

    private void LSW_Old_Shield(LivingEntity target, Action onCastEnd = null)
    {
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

        Anim_M_Shield shieldEffect = PoolManager.GetItem<Anim_M_Shield>();
        shieldEffect.transform.position = owner.transform.position;

        shieldEffect.Play(() =>
        {
            onCastEnd?.Invoke();
        });

        owner.GetComponent<EnemyHealth>().AddShield(15);
    }


}
