using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_C_Shield_Throwing : SkillPiece
{
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        LivingEntity health = Owner.GetComponent<LivingEntity>();

        InventoryHandler ih = GameManager.Instance.inventoryHandler;

        if (health.HasShield())
        {
            int shieldDamage = health.GetShieldHp();

            health.RemoveShield();

            int shield = Mathf.Clamp(shieldDamage / 3, 1, 10);

            for (int i = 0; i < shield; i++)
            {
                int a = i;

                EffectObj effect = PoolManager.GetItem<EffectObj>();
                effect.SetSprite(GameManager.Instance.buffIcons[0]);
                effect.SetColorGradient(ih.effectGradDic[currentType]);
                effect.SetScale(Vector3.one * 4f);

                effect.transform.position = Owner.transform.position;

                effect.transform.DOMove(UnityEngine.Random.insideUnitCircle * 1.4f, 0.25f)
                .SetRelative();

                effect.Play(target.transform.position, () =>
                {
                    GameManager.Instance.cameraHandler.ShakeCamera(2.5f + (a * 0.1f), 0.2f);

                    animHandler.GetAnim(AnimName.M_Butt)
                    .SetPosition(target.transform.position)
                    .SetScale(1f)
                    .Play();

                    if (a == shield - 1)
                    {
                        target.GetDamage(shieldDamage);

                        onCastEnd?.Invoke();
                    }

                    effect.EndEffect();
                }
                , BezierType.Quadratic, isRotate:true, playSpeed:2.15f, delay:0.25f);
            }
        }
        else
        {
            onCastEnd?.Invoke();
        }
    }
}
