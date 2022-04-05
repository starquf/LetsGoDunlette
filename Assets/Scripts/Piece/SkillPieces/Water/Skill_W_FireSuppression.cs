using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_W_FireSuppression : SkillPiece
{
    public Sprite manaSphereSpr;
    private Gradient effectGradient;

    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);
    private readonly WaitForSeconds pTwoSecWait = new WaitForSeconds(0.2f);

    private BattleHandler bh;

    protected override void Start()
    {
        base.Start();

        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[PatternType.Spade];
        bh = GameManager.Instance.battleHandler;

        hasTarget = true;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        StartCoroutine(Tsunami(target, onCastEnd));
    }

    private IEnumerator Tsunami(LivingEntity target, Action onCastEnd = null)
    {
        int waterCnt = 0;

        Vector3 startPos = bh.bottomPos.position;

        Rullet rullet = bh.mainRullet;
        List<RulletPiece> skillPieces = rullet.GetPieces();

        for (int i = 0; i < skillPieces.Count; i++)
        {
            if (skillPieces[i] == null) continue;

            if (skillPieces[i].PieceType.Equals(PieceType.SKILL))
            {
                if (skillPieces[i].currentType.Equals(PatternType.Heart) && skillPieces[i] != this)
                {
                    Vector3 skillPos = skillPieces[i].skillImg.transform.position;
                    int a = i;

                    waterCnt++;

                    EffectObj effect = PoolManager.GetItem<EffectObj>();
                    effect.transform.position = startPos;
                    effect.SetSprite(manaSphereSpr);
                    effect.SetColorGradient(effectGradient);
                    effect.SetScale(Vector3.one);

                    effect.Play(skillPos, () => {

                        Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
                        textEffect.SetType(TextUpAnimType.Damage);
                        textEffect.transform.position = skillPieces[a].skillImg.transform.position;
                        textEffect.Play("화재진압 발동!");

                        Anim_W_Splash1 splashEffect = PoolManager.GetItem<Anim_W_Splash1>();
                        splashEffect.transform.position = skillPos;
                        splashEffect.SetScale(0.5f);

                        GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.15f, 0.1f);

                        splashEffect.Play();

                        GameManager.Instance.battleHandler.mainRullet.PutRulletPieceToGraveYard(a);
                        effect.EndEffect();
                    }, BezierType.Quadratic, isRotate: true, playSpeed: 2.3f);

                    yield return pTwoSecWait;
                }
            }
        }

        yield return pTwoSecWait;

        EffectObj skillEffect = PoolManager.GetItem<EffectObj>();
        skillEffect.transform.position = startPos;
        skillEffect.SetSprite(manaSphereSpr);
        skillEffect.SetColorGradient(effectGradient);
        skillEffect.SetScale(Vector3.one * (waterCnt*0.3f + 1));

        skillEffect.Play(target.transform.position, () => {
            target.GetDamage(Value, currentType);

            Anim_W_Splash splashEffect = PoolManager.GetItem<Anim_W_Splash>();
            splashEffect.transform.position = target.transform.position;

            GameManager.Instance.cameraHandler.ShakeCamera(1.5f + waterCnt * 0.2f, 0.15f);

            splashEffect.Play();

            if(waterCnt <= 0)
            {
                onCastEnd?.Invoke();
            }
            skillEffect.EndEffect();
        }, BezierType.Linear, isRotate: true, playSpeed: 2f);
        for (int i = 0; i < waterCnt; i++)
        {
            yield return new WaitForSeconds(0.1f);
            int a = i;
            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.transform.position = startPos;
            effect.SetSprite(manaSphereSpr);
            effect.SetColorGradient(effectGradient);
            effect.SetScale(Vector3.one);

            effect.Play(target.transform.position, () => {

                target.GetDamage(40, currentType);

                Anim_W_Splash1 splashEffect = PoolManager.GetItem<Anim_W_Splash1>();
                splashEffect.transform.position = target.transform.position;

                GameManager.Instance.cameraHandler.ShakeCamera(1f, 0.15f);

                splashEffect.Play();

                if (a == waterCnt - 1)
                {
                    onCastEnd?.Invoke();
                }
                effect.EndEffect();
            }, BezierType.Cubic, isRotate: true, playSpeed: 2f);
        }
    }
}
