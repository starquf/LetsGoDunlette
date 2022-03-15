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
            target.GetDamage(Value, owner.gameObject);
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            Anim_M_Butt hitEffect = PoolManager.GetItem<Anim_M_Butt>();
            hitEffect.transform.position = owner.transform.position;

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }
}
