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

    protected override void Start()
    {
        base.Start();

        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.Water];
        bh = GameManager.Instance.battleHandler;

        isTargeting = true;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc()}");

        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        bh.battleUtil.StartCoroutine(FireSuppression(target, onCastEnd));
    }

    private IEnumerator FireSuppression(LivingEntity target, Action onCastEnd = null)
    {
        int waterCnt = 0;

        Vector3 startPos = bh.bottomPos.position;

        Rullet rullet = bh.mainRullet;
        List<RulletPiece> skillPieces = rullet.GetPieces();

        for (int i = 0; i < skillPieces.Count; i++)
        {
            if (skillPieces[i] == null)
            {
                continue;
            }

            if (skillPieces[i].PieceType.Equals(PieceType.SKILL))
            {
                if (skillPieces[i].currentType.Equals(ElementalType.Fire) && skillPieces[i] != this)
                {
                    Vector3 skillPos = skillPieces[i].skillImg.transform.position;
                    int a = i;

                    waterCnt++;

                    EffectObj effect = PoolManager.GetItem<EffectObj>();
                    effect.transform.position = startPos;
                    effect.SetSprite(manaSphereSpr);
                    effect.SetColorGradient(effectGradient);
                    effect.SetScale(Vector3.one);

                    effect.Play(skillPos, () =>
                    {

                        animHandler.GetTextAnim()
                        .SetType(TextUpAnimType.Up)
                        .SetPosition(skillPieces[a].skillImg.transform.position)
                        .Play("화재진압 발동!");

                        animHandler.GetAnim(AnimName.W_Splash02)
                        .SetPosition(skillPos)
                        .SetScale(0.5f)
                        .Play();

                        GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.15f, 0.1f);

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
        skillEffect.SetScale(Vector3.one * ((waterCnt * 0.3f) + 1));

        int damage = GetDamageCalc();

        skillEffect.Play(target.transform.position, () =>
        {
            target.GetDamage(damage, currentType);

            GameManager.Instance.cameraHandler.ShakeCamera(1.5f + (waterCnt * 0.2f), 0.15f);

            animHandler.GetAnim(AnimName.W_Splash01)
                    .SetPosition(target.transform.position)
                    .Play();

            if (waterCnt <= 0)
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

            effect.Play(target.transform.position, () =>
            {

                target.GetDamage(3, patternType);

                GameManager.Instance.cameraHandler.ShakeCamera(1f, 0.15f);

                animHandler.GetAnim(AnimName.W_Splash02)
                .SetPosition(target.transform.position)
                .SetScale(0.5f)
                .Play();

                if (a == waterCnt - 1)
                {
                    onCastEnd?.Invoke();
                }
                effect.EndEffect();
            }, BezierType.Cubic, isRotate: true, playSpeed: 2f);
        }
    }
}
