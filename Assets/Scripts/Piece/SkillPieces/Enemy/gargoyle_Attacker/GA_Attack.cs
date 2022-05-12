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
        SetIndicator(owner.gameObject, "공격").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            target.GetDamage(Value, this, owner);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                SetIndicator(owner.gameObject, "보호막").OnEndAction(() =>
                {
                    owner.GetComponent<EnemyHealth>().AddShield(10);
                    animHandler.GetAnim(AnimName.M_Shield).SetPosition(owner.transform.position)
            .SetScale(1)
            .Play(() =>
                    {
                        onCastEnd?.Invoke();
                    });
                });
            });
        });
    }
}
