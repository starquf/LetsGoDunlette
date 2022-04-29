using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_E_LightningRod : SkillPiece
{
    public Sprite effectSpr;
    private Gradient effectGradient;

    protected override void Start()
    {
        base.Start();
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.Electric];
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());

        return desInfos;
    }

    private int GetDamageCalc()
    {
        int attack = (int)(owner.GetComponent<LivingEntity>().AttackPower * 0.1f + 1);

        return attack;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        //print($"스킬 발동!! 이름 : {PieceName}");
        BattleHandler battleHandler = GameManager.Instance.battleHandler;

        Rullet rullet = battleHandler.mainRullet;
        List<RulletPiece> skillPieces = rullet.GetPieces();

        List<SkillPiece> lightningSkillPieces = new List<SkillPiece>();
        Dictionary<SkillPiece, int> lightningSkillIdxDic = new Dictionary<SkillPiece, int>();

        for (int i = 0; i < skillPieces.Count; i++)
        {
            if (skillPieces[i] == null) continue;

            if(skillPieces[i].PieceType.Equals(PieceType.SKILL))
            {
                if(skillPieces[i].currentType.Equals(ElementalType.Electric) && skillPieces[i] != this)
                {
                    SkillPiece piece = skillPieces[i] as SkillPiece;
                    lightningSkillPieces.Add(piece);
                    lightningSkillIdxDic.Add(piece, i);
                }
            }
        }

        SkillPiece result = null;

        // 번개 속성이 존재한다면
        if (lightningSkillPieces.Count > 0)
        {
            result = lightningSkillPieces[Random.Range(0, lightningSkillPieces.Count)];
            result.HighlightColor(0.4f);

            Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
            textEffect.SetType(TextUpAnimType.Up);
            textEffect.transform.position = result.skillImg.transform.position;
            textEffect.Play("피뢰침 효과발동!");

            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.transform.position = skillImg.transform.position;
            effect.SetSprite(effectSpr);
            effect.SetColorGradient(effectGradient);
            effect.SetScale(Vector3.one * 0.6f);

            effect.Play(result.skillImg.transform.position, () => {
                effect.EndEffect();
            }, BezierType.Linear, isRotate: true, playSpeed: 3f);
        }

        Vector3 targetPos = target.transform.position;
        targetPos.y -= 0.7f;
        targetPos.x += 0.5f;

        animHandler.GetAnim(AnimName.E_LightningRod)
            .SetScale(0.5f)
            .SetPosition(targetPos)
            .Play(() =>
            {
                // 번개 속성이 존재한다면
                if (result != null)
                {
                    battleHandler.battleEvent.StartActionEvent(EventTimeSkill.WithSkill, result);

                    LogCon log = new LogCon();
                    log.text = $"스킬 발동";
                    log.selfSpr = skillImg.sprite;
                    log.targetSpr = result.skillImg.sprite;

                    DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);

                    result.Cast(target, onCastEnd);

                    battleHandler.battleUtil.SetPieceToGraveyard(lightningSkillIdxDic[result]);

                    log = new LogCon();
                    log.text = $"무덤으로 보냄";
                    log.selfSpr = skillImg.sprite;
                    log.targetSpr = result.skillImg.sprite;

                    DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);
                }
                else
                {
                    onCastEnd?.Invoke();
                }
            });

        int damage = GetDamageCalc();

        target.GetDamage(damage, currentType);

        LogCon log = new LogCon();
        log.text = $"{damage} 데미지 부여";
        log.selfSpr = skillImg.sprite;
        log.targetSpr = target.GetComponent<SpriteRenderer>().sprite;

        DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);

        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
    }
}
