using System;
using Random = UnityEngine.Random;

public class MI_Skill : SkillPiece
{
    public int bittingDamage = 15;
    public int bumpDamage = 35;

    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        if (Random.Range(0, 100) <= value)
        {
            MI_Biting(target, onCastEnd);
        }
        else
        {
            MI_Bump(target, onCastEnd);
        }
    }

    private void MI_Biting(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "����").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            Anim_M_Bite hitEffect = PoolManager.GetItem<Anim_M_Bite>();
            hitEffect.transform.position = GameManager.Instance.enemyEffectTrm.position; hitEffect.SetScale(2);

            target.GetDamage(bittingDamage, this, owner);
            hitEffect.Play(() =>
            {
                SetIndicator(owner.gameObject, "��ó�ο�").OnEnd(() =>
                {
                    target.cc.SetCC(CCType.Wound, 5);
                    onCastEnd?.Invoke();
                });
            });
        });
    }

    private void MI_Bump(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "����").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.7f, 0.15f);

            target.GetDamage(bumpDamage, this, owner);

            Anim_M_Bite hitEffect = PoolManager.GetItem<Anim_M_Bite>();
            hitEffect.transform.position = GameManager.Instance.enemyEffectTrm.position; hitEffect.SetScale(2);

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }


}
