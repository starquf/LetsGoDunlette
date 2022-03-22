using System;
using Random = UnityEngine.Random;

public class MD_Skill : SkillPiece
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
            MD_Hallucinations(target, onCastEnd);
        }
        else
        {
            MD_Scream(target, onCastEnd);
        }
    }

    private void MD_Hallucinations(LivingEntity target, Action onCastEnd = null) //40% 확률로 침묵을 부여한다.
    {
        SetIndicator(owner.gameObject, "공격").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
            effect.transform.position = owner.transform.position;

            target.GetDamage(15, this, owner);

            effect.Play(() =>
            {
                if (Random.Range(1, 100) <= 40)
                {
                    SetIndicator(owner.gameObject, "침묵부여").OnEnd(() =>
                    {
                        target.cc.SetCC(CCType.Silence, 2);

                        Anim_M_Recover effect1 = PoolManager.GetItem<Anim_M_Recover>();
                        effect1.transform.position = owner.transform.position;
                        effect1.Play(() =>
                        {
                            onCastEnd?.Invoke();
                        });
                    });
                }
                else
                {
                    onCastEnd?.Invoke();
                }
            });
        });
    }

    private void MD_Scream(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "공격").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
            effect.transform.position = owner.transform.position;

            effect.Play(() =>
            {
                onCastEnd?.Invoke();
            });

            target.GetDamage(35, this, owner);
        });
    }


}
