using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_N_Pigeon : SkillPiece
{
    public Sprite pigeonSpr;
    private Gradient effectGradient;

    private BattleHandler bh;

    protected override void Start()
    {
        base.Start();
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.Nature];

        bh = GameManager.Instance.battleHandler;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) //적에게 60피해를 입힌다.		침묵	-	적에게 2턴간 침묵을 부여한다.
    {
        Vector3 targetPos = target.transform.position;
        Vector3 startPos = bh.bottomPos.position;

        for (int i = 0; i < 10; i++)
        {
            int a = i;

            startPos.x = Random.Range(-1.8f, 1.8f);

            EffectObj skillEffect = PoolManager.GetItem<EffectObj>();
            skillEffect.transform.position = startPos;
            skillEffect.SetSprite(pigeonSpr);
            skillEffect.SetColorGradient(effectGradient);
            skillEffect.SetScale(Vector3.one * 0.7f);

            skillEffect.Play(targetPos, () =>
            {
                Anim_N_ManaSphereHit hitEffect = PoolManager.GetItem<Anim_N_ManaSphereHit>();
                hitEffect.SetScale(0.5f);
                hitEffect.transform.position = targetPos;

                target.GetDamage(value / 10, currentType);

                GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
                hitEffect.Play();

                if (a >= 9)
                {
                    target.cc.SetCC(CCType.Silence, 3);
                    onCastEnd?.Invoke();
                }

                skillEffect.EndEffect();
            }, BezierType.Cubic, isRotate: true, playSpeed: 2f, delay: i * 0.05f);
        }
    }
}
