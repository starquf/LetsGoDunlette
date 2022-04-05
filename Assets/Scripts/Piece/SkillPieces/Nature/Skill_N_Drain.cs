using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_N_Drain : SkillPiece
{
    public Sprite drainingEffectSpr;
    private Gradient effectGradient;

    [Header("파라매터들")]
    public int healValue = 15;

    protected override void Start()
    {
        base.Start();

        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[PatternType.Clover];

        hasTarget = true;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) //적에게 50의 피해를 입힌다.		-	회복	체력을 20 회복한다.
    {
        BattleHandler bh = GameManager.Instance.battleHandler;

        bh.battleUtil.StartCoroutine(Drain(target, onCastEnd));
    }

    private IEnumerator Drain(LivingEntity target, Action onCastEnd = null)
    {
        target.GetDamage(value, currentType);

        Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
        textEffect.SetType(TextUpAnimType.Damage);
        textEffect.transform.position = target.transform.position;
        textEffect.Play("흡수!");

        Anim_N_Drain skillEffect = PoolManager.GetItem<Anim_N_Drain>();
        skillEffect.transform.position = target.transform.position;
        skillEffect.SetScale(1f);

        GameManager.Instance.cameraHandler.ShakeCamera(2f, 0.3f);
        skillEffect.Play();

        yield return new WaitForSeconds(0.1f);

        LivingEntity healTarget = owner.GetComponent<LivingEntity>();

        const float time = 0.8f;
        int rand = Random.Range(7, 13);
        int healAmount = 0;
        for (int i = 0; i < rand; i++)
        {
            int a = i;
            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.transform.position = target.transform.position;
            effect.SetSprite(drainingEffectSpr);
            effect.SetColorGradient(effectGradient);
            effect.SetScale(Vector3.one * 0.5f);

            effect.Play(healTarget.transform.position, () => {

                Anim_M_Recover skillEffect = PoolManager.GetItem<Anim_M_Recover>();
                skillEffect.transform.position = effect.transform.position;
                skillEffect.SetScale(0.4f);

                skillEffect.Play();

                if(a == rand -1)
                {
                    healTarget.Heal(healValue - healAmount);
                    onCastEnd?.Invoke();
                }
                else
                {
                    int heal = (healValue - healAmount) / rand;
                    healAmount += heal;
                    healTarget.Heal(heal);
                }

                effect.EndEffect();
            }, BezierType.Quadratic, isRotate: true, playSpeed: 1.5f);
            yield return new WaitForSeconds(time / (float)rand);
        }
    }
}
