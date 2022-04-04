using System;

public class GA_Attack : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "공격").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            target.GetDamage(Value, this, owner);

            Anim_M_Sword hitEffect = PoolManager.GetItem<Anim_M_Sword>();
            hitEffect.transform.position = GameManager.Instance.enemyEffectTrm.position; hitEffect.SetScale(2);

            hitEffect.Play(() =>
            {
                SetIndicator(owner.gameObject, "보호막").OnEnd(() =>
                {
                    Anim_M_Shield effect = PoolManager.GetItem<Anim_M_Shield>();
                    effect.transform.position = owner.transform.position;

                    owner.GetComponent<EnemyHealth>().AddShield(10);

                    effect.Play(() =>
                    {
                        onCastEnd?.Invoke();
                    });
                });
            });
        });
    }
}
