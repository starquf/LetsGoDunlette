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
        SetIndicator(owner.gameObject, "공격").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            Anim_BuffEffect03 hitEffect = PoolManager.GetItem<Anim_BuffEffect03>();
            hitEffect.transform.position = GameManager.Instance.enemyEffectTrm.position; hitEffect.SetScale(1.5f);

            if(owner.GetComponent<EnemyHealth>().GetHpRatio() >= 50)
            {
                if(Random.Range(0, 100) < 40)
                {

                    Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
                    textEffect.SetType(TextUpAnimType.Damage);
                    textEffect.transform.position = target.transform.position;
                    textEffect.SetScale(0.7f);
                    textEffect.Play("기절!");

                    target.cc.SetCC(CCType.Stun, 1);
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
