using System;
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
        if (Random.Range(0, 100) <= value)
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
        SetIndicator(owner.gameObject, "공격").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            target.GetDamage(30, this, owner);

            Anim_M_Sword hitEffect = PoolManager.GetItem<Anim_M_Sword>();
            hitEffect.transform.position = owner.transform.position;

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    private void LSW_Old_Shield(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "보호막").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            Anim_M_Shield shieldEffect = PoolManager.GetItem<Anim_M_Shield>();
            shieldEffect.transform.position = owner.transform.position;

            shieldEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });

            owner.GetComponent<EnemyHealth>().AddShield(15);
        });
    }


}
