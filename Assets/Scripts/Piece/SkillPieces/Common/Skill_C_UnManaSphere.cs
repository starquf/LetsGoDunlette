using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_C_UnManaSphere : SkillPiece
{
    public Sprite manaSphereSpr;
    private Gradient effectGradient;

    protected override void Start()
    {
        base.Start();
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.None];
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc()}");

        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = target.transform.position;

        animHandler.GetAnim(AnimName.C_SphereCast)
            .SetPosition(startPos)
            .Play(() =>
            {
                int damage = GetDamageCalc();

                int rand = Random.Range(0, 100);

                for (int i = 0; i < 2; i++)
                {
                    int a = i;

                    EffectObj effect = PoolManager.GetItem<EffectObj>();
                    effect.transform.position = startPos;
                    effect.SetSprite(manaSphereSpr);
                    effect.SetColorGradient(effectGradient);
                    effect.SetScale(Vector3.one * Random.Range(0.6f, 1f));

                    effect.Play(targetPos, () =>
                    {
                        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

                        animHandler.GetAnim(AnimName.C_ManaSphereHit)
                        .SetPosition(targetPos)
                        .Play();

                        if (a == 1)
                        {
                            target.GetDamage(damage, currentType);

                            if (!CheckCC(CCType.Silence) && rand < 35)
                            {
                                Owner.GetComponent<LivingEntity>().cc.SetCC(CCType.Silence, 2);
                            }

                            onCastEnd?.Invoke();
                        }

                        effect.EndEffect();

                    }, BezierType.Quadratic, isRotate: true, playSpeed: 1.7f);
                }
            });
    }
}
