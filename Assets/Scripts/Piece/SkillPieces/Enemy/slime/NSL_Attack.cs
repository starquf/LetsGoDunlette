using System;

public class NSL_Attack : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "АјАн").OnEnd(() =>
        {
            target.GetDamage(Value, this,owner);
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            animHandler.GetAnim(AnimName.M_Butt)
            .SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2f)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }
}
