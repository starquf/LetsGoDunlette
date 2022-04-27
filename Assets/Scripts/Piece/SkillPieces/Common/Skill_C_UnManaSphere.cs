using System;
using System.Collections;
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

        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc().ToString()}");

        return desInfos;
    }

    private int GetDamageCalc()
    {
        int attack = (int)(owner.GetComponent<LivingEntity>().AttackPower * 0.6f);

        return attack;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = target.transform.position;

        Anim_C_SphereCast castAnim = PoolManager.GetItem<Anim_C_SphereCast>();
        castAnim.transform.position = startPos;

        castAnim.Play(() =>
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

                    //print($"데미지 발동 : {damage}");
                    Anim_C_ManaSphereHit hitEffect = PoolManager.GetItem<Anim_C_ManaSphereHit>();
                    hitEffect.transform.position = targetPos;

                    hitEffect.Play(() =>
                    {
                    });

                    if (a == 1)
                    {
                        target.GetDamage(damage, currentType);

                        if (!CheckSilence() && rand < 35)
                        {
                            owner.GetComponent<LivingEntity>().cc.SetCC(CCType.Silence, 2);
                        }

                        onCastEnd?.Invoke();
                    }

                    effect.EndEffect();

                }, BezierType.Quadratic, isRotate: true, playSpeed: 1.7f);
            }
        });
    }
}
