using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GR_Skill : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        GR_StrangeLight(target, onCastEnd);
    }

    private void GR_StrangeLight(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "АјАн").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            Anim_M_Bite hitEffect = PoolManager.GetItem<Anim_M_Bite>();
            hitEffect.transform.position = GameManager.Instance.enemyEffectTrm.position; hitEffect.SetScale(2);

            if(owner.GetComponent<EnemyHealth>().GetHpRatio() >= 50)
            {
                if(Random.Range(0, 100) < 40)
                {
                    target.cc.SetCC(CCType.Stun,1);
                }
            }

            target.GetDamage(Value, this, owner);
            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }
}
