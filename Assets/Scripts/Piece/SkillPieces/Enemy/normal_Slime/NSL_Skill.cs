using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class NSL_Skill : SkillPiece
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
            NSl_Recover(target, onCastEnd);
        }
        else
        {
            NSL_Bounce(target, onCastEnd);
        }
    }

    private void NSl_Recover(LivingEntity target, Action onCastEnd = null)
    {
        GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

        Vector3 targetPos = owner.transform.position;

        Anim_M_Recover recoverEffect = PoolManager.GetItem<Anim_M_Recover>();
        recoverEffect.transform.position = targetPos;

        recoverEffect.Play(() =>
        {
            onCastEnd?.Invoke();
        });

        owner.GetComponent<EnemyHealth>().Heal(30);
    }

    private void NSL_Bounce(LivingEntity target, Action onCastEnd = null)
    {
        GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

        target.GetDamage(30, owner.gameObject);

        Anim_M_Butt hitEffect = PoolManager.GetItem<Anim_M_Butt>();
        hitEffect.transform.position = owner.transform.position;

        hitEffect.Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }


}
