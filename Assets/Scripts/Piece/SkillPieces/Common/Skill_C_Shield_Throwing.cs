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
            int shield = health.GetShieldHp();

            target.GetDamage(shield);
            health.RemoveShield();

            shield = Mathf.Clamp(shield / 3, 1, 10);

            for (int i = 0; i < shield; i++)
            {
                EffectObj effect = PoolManager.GetItem<EffectObj>();
                effect.SetSprite(GameManager.Instance.buffIcons[0]);
                effect.SetColorGradient(ih.effectGradDic[currentType]);

                effect.transform.position = skillIconImg.transform.position;

                effect.transform.DOMove(UnityEngine.Random.insideUnitCircle * 1.5f, 0.4f)
                    .SetRelative();

                effect.Play(target.transform.position, () =>
                {
                    GameManager.Instance.cameraHandler.ShakeCamera(0.5f + (i * 0.1f), 0.2f);

                    effect.EndEffect();
                }
                , BezierType.Quadratic, 0.4f, isRotate:true);
            }
        }

        animHandler.GetAnim(AnimName.E_ManaSphereHit)
            .SetScale(0.5f)
            .SetPosition(skillIconImg.transform.position)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
    }
}
