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
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[PatternType.None];
    }

    public override void Cast(Action onCastEnd = null)
    {
        Vector3 startPos = GameManager.Instance.battleHandler.player.transform.position;
        Vector3 target = GameManager.Instance.battleHandler.enemy.transform.position;

        Anim_C_SphereCast castAnim = PoolManager.GetItem<Anim_C_SphereCast>();
        castAnim.transform.position = startPos;

        castAnim.Play(() =>
        {
            PlayerAttackAnimation();

            int damage = Value / 2;

            int rand = Random.Range(0, 100);

            for (int i = 0; i < 2; i++)
            {
                int a = i;

                EffectObj effect = PoolManager.GetItem<EffectObj>();
                effect.transform.position = startPos;
                effect.SetSprite(manaSphereSpr);
                effect.SetColorGradient(effectGradient);

                effect.Play(target, () =>
                {
                    GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

                    print($"데미지 발동 : {damage}");
                    GameManager.Instance.battleHandler.enemy.GetDamage(damage);

                    Anim_C_ManaSphereHit hitEffect = PoolManager.GetItem<Anim_C_ManaSphereHit>();
                    hitEffect.transform.position = target;

                    hitEffect.Play(() =>
                    {
                    });

                    if (a == 1)
                    {
                        if (!CheckSilence() && rand < 35)
                        {
                            GameManager.Instance.battleHandler.player.cc.SetCC(CCType.Silence, 4);
                        }

                        onCastEnd?.Invoke();
                    }

                    effect.EndEffect();

                }, BezierType.Quadratic, isRotate: true, delay: i * 0.05f);
            }
        });
    }
}
