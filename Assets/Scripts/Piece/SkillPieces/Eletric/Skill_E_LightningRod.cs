using System;
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

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        //print($"��ų �ߵ�!! �̸� : {PieceName}");
        Rullet rullet = bh.mainRullet;
        List<RulletPiece> skillPieces = rullet.GetPieces();

        List<SkillPiece> lightningSkillPieces = new List<SkillPiece>();
        Dictionary<SkillPiece, int> lightningSkillIdxDic = new Dictionary<SkillPiece, int>();

        for (int i = 0; i < skillPieces.Count; i++)
        {
            if (skillPieces[i] == null)
            {
                continue;
            }

            if (skillPieces[i].PieceType.Equals(PieceType.SKILL))
            {
                if (skillPieces[i].currentType.Equals(ElementalType.Electric) && skillPieces[i] != this)
                {
                    SkillPiece piece = skillPieces[i] as SkillPiece;
                    lightningSkillPieces.Add(piece);
                    lightningSkillIdxDic.Add(piece, i);
                }
            }
        }

        SkillPiece result = null;

        // ���� �Ӽ��� �����Ѵٸ�
        if (lightningSkillPieces.Count > 0)
        {
            result = lightningSkillPieces[Random.Range(0, lightningSkillPieces.Count)];
            result.HighlightColor(0.4f);

            animHandler.GetTextAnim()
            .SetType(TextUpAnimType.Up)
            .SetPosition(result.skillIconImg.transform.position)
            .Play("�Ƿ�ħ ȿ���ߵ�!");

            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.transform.position = skillIconImg.transform.position;
            effect.SetSprite(effectSpr);
            effect.SetColorGradient(effectGradient);
            effect.SetScale(Vector3.one * 0.6f);

            effect.Play(result.skillIconImg.transform.position, () =>
            {
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
                // ���� �Ӽ��� �����Ѵٸ�
                if (result != null)
                {
                    bh.battleEvent.StartActionEvent(EventTimeSkill.WithSkill, result);

                    LogCon log = new LogCon
                    {
                        text = $"��ų �ߵ�",
                        selfSpr = skillIconImg.sprite,
                        targetSpr = result.skillIconImg.sprite
                    };

                    DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);

                    result.Cast(target, onCastEnd);

                    bh.battleUtil.SetPieceToGraveyard(lightningSkillIdxDic[result]);

                    log = new LogCon
                    {
                        text = $"�������� ����",
                        selfSpr = skillIconImg.sprite,
                        targetSpr = result.skillIconImg.sprite
                    };

                    DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);
                }
                else
                {
                    onCastEnd?.Invoke();
                }
            });

        int damage = GetDamageCalc();

        target.GetDamage(damage, currentType);

        LogCon log = new LogCon
        {
            text = $"{damage} ������ �ο�",
            selfSpr = skillIconImg.sprite,
            targetSpr = target.GetComponent<SpriteRenderer>().sprite
        };

        DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);

        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
    }
}
