using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_E_Lightning : SkillPiece
{
    private BattleHandler bh = null;

    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        target.GetDamage(value);

        Anim_E_Static effect = PoolManager.GetItem<Anim_E_Static>();
        effect.transform.position = target.transform.position;
        effect.SetScale(1.2f);

        GameManager.Instance.cameraHandler.ShakeCamera(2f, 0.2f);

        effect.Play(() =>
        {
            PlayerHealth playerHealth = owner.GetComponent<PlayerHealth>();
            if (playerHealth.HasShield())
            {
                if (Random.Range(0, 100) < 60)
                {
                    Anim_E_Static_Stun stunEffect = PoolManager.GetItem<Anim_E_Static_Stun>();
                    stunEffect.transform.position = bh.playerImgTrans.position;

                    stunEffect.Play();

                    playerHealth.cc.SetCC(CCType.Stun, 1);
                }
            }
            else
            {
                Anim_E_Static_Stun stunEffect = PoolManager.GetItem<Anim_E_Static_Stun>();
                stunEffect.transform.position = target.transform.position;

                stunEffect.Play();

                target.cc.SetCC(CCType.Stun, 1);
            }

            onCastEnd?.Invoke();
        });

    }
}
