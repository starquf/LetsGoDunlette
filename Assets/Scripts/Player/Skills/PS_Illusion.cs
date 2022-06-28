using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_Illusion : PlayerSkill_Cooldown
{
    public Sprite attackSpr;

    private Gradient attackGrad;
    private AnimHandler animHandler;

    private WaitForSeconds atkWait = new WaitForSeconds(0.22f);
    private WaitForSeconds pFiveSecWait = new WaitForSeconds(0.5f);

    protected override void Start()
    {
        base.Start();

        attackGrad = GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.None];
        animHandler = GameManager.Instance.animHandler;
    }

    public override void Cast(Action onEndSkill, Action onCancelSkill)
    {
        base.Cast(onEndSkill, onCancelSkill);

        onEndSkill += () =>
        {
            ResetCooldown();
        };

        bh.mainRullet.PauseRullet();

        StartCoroutine(SkillActivate(onEndSkill));
    }

    private IEnumerator SkillActivate(Action onEndSkill)
    {
        Rullet rullet = bh.mainRullet;
        List<RulletPiece> skillPieces = rullet.GetPieces();

        List<LivingEntity> targets = new List<LivingEntity>();
        targets = bh.battleUtil.DeepCopyEnemyList(bh.enemys);

        int damage = 3;

        for (int i = 0; i < skillPieces.Count; i++)
        {
            int a = i;

            if (skillPieces[i] == null)
            {
                continue;
            }

            SkillPiece sp = skillPieces[i] as SkillPiece;

            if (sp.isPlayerSkill)
            {
                Vector3 skillPos = sp.skillIconImg.transform.position;
                animHandler.GetTextAnim()
                        .SetType(TextUpAnimType.Up)
                        .SetPosition(sp.skillIconImg.transform.position)
                        .Play($"{skillName} 발동!");

                LivingEntity enemy = targets[UnityEngine.Random.Range(0, targets.Count)];

                // 공격 로직
                AttackEnemy(skillPos, enemy, damage);

                GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.15f, 0.1f);
                bh.mainRullet.PutRulletPieceToGraveYard(a);

                yield return atkWait;
            }
        }

        yield return pFiveSecWait;

        if (bh.CheckBattleEnd())
        {
            onEndSkill?.Invoke();
        }
        else
        {
            StartCoroutine(bh.battleUtil.ResetRullet(() =>
            {
                StartCoroutine(bh.CheckPanelty(onEndSkill));
            }, 
            sp => 
            {
                animHandler.GetAnim(AnimName.GothicEffect08)
                    .SetPosition(sp.skillIconImg.transform.position)
                    .SetScale(1.4f)
                    .Play();
            }));
        }
    }

    private void AttackEnemy(Vector3 startPos, LivingEntity target, int damage)
    {
        animHandler.GetAnim(AnimName.GothicEffect08)
            .SetPosition(startPos)
            .SetScale(1.4f)
            .Play();

        EffectObj effect = PoolManager.GetItem<EffectObj>();
        effect.transform.position = startPos;
        effect.SetSprite(attackSpr);
        effect.SetColorGradient(attackGrad);
        effect.SetScale(Vector3.one * 1.4f);

        effect.Play(target.transform.position, () =>
        {
            target.GetDamage(damage, ElementalType.None);

            animHandler.GetAnim(AnimName.GothicEffect08)
            .SetPosition(target.transform.position)
            .SetScale(1.2f)
            .Play();

            GameManager.Instance.cameraHandler.ShakeCamera(1.5f, 0.1f);

            effect.EndEffect();
        }, BezierType.Quadratic, playSpeed:1.85f, isRotate:true);
    }
}
