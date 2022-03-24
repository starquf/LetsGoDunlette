using System;
using Random = UnityEngine.Random;

public class NSL_Skill : SkillPiece
{
    [UnityEngine.Header("스킬 벨류")]
    public int BounceDmg = 25;
    public int recoverVal = 30;

    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        if (Random.Range(0, 100) <= value)
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
        SetIndicator(owner.gameObject, "회복").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            Anim_M_Recover recoverEffect = PoolManager.GetItem<Anim_M_Recover>();
            recoverEffect.transform.position = owner.transform.position;

            recoverEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });

            owner.GetComponent<EnemyHealth>().Heal(recoverVal);
        });
    }

    private void NSL_Bounce(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "공격").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            target.GetDamage(BounceDmg, this, owner);

            Anim_M_Butt hitEffect = PoolManager.GetItem<Anim_M_Butt>();
            hitEffect.transform.position = GameManager.Instance.enemyEffectTrm.position; hitEffect.SetScale(2);

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }


}
