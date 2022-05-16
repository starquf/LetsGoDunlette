using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_W_Tsunami : SkillPiece
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

        isTargeting = false;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());

        return desInfos;
    }

    private int GetDamageCalc()
    {
        int attack = (int)(Owner.GetComponent<LivingEntity>().AttackPower * 0.4f);

        return attack;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        GameManager.Instance.battleHandler.battleUtil.StartCoroutine(Tsunami(target, onCastEnd));
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
                if (skillPieces[i].currentType.Equals(ElementalType.Water) && skillPieces[i] != this)
                {
                    Vector3 skillPos = skillPieces[i].skillImg.transform.position;
                    int a = i;

                    waterCnt++;

                    animHandler.GetAnim(AnimName.W_Splash01)
                    .SetPosition(skillPos)
                    .SetScale(0.5f)
                    .Play();

                    EffectObj effect = PoolManager.GetItem<EffectObj>();
                    effect.transform.position = skillPos;
                    effect.SetSprite(manaSphereSpr);
                    effect.SetColorGradient(effectGradient);
                    effect.SetScale(Vector3.one);

                    effect.Play(startPos, () => {
                        GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.15f, 0.1f);

                        animHandler.GetAnim(AnimName.W_Splash01)
                        .SetPosition(startPos)
                        .SetScale(0.5f)
                        .Play();

                        effect.EndEffect();
                    }, BezierType.Quadratic, isRotate: true, playSpeed: 2f);

                    yield return pTwoSecWait;
                }
            }
        }

        yield return pTwoSecWait;

        EffectObj skillEffect = PoolManager.GetItem<EffectObj>();
        skillEffect.transform.position = startPos;
        skillEffect.SetSprite(manaSphereSpr);
        skillEffect.SetColorGradient(effectGradient);
        skillEffect.SetScale(Vector3.one * (waterCnt + 1));

        skillEffect.Play(bh.createTrans.position, () => {

            List<LivingEntity> targets = new List<LivingEntity>();

            if (target == bh.player)
            {
                targets.Add(target);
            }
            else
            {
                targets = bh.battleUtil.DeepCopyEnemyList(bh.enemys);
            }

            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].GetDamage(GetDamageCalc() + (2 * waterCnt), currentType);

                GameManager.Instance.cameraHandler.ShakeCamera(1.5f + waterCnt * 0.5f, 0.15f);

                animHandler.GetAnim(AnimName.W_Splash01)
                        .SetPosition(targets[i].transform.position)
                        .SetScale(0.5f)
                        .Play();
            }

            onCastEnd?.Invoke();

            skillEffect.EndEffect();
        }, BezierType.Linear, isRotate: true, playSpeed: 1.8f);
    }
}
