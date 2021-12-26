using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_E_LightningRod : SkillPiece
{
    public GameObject LightningRodEffectPrefab;

    public override void Cast(Action onCastEnd = null)
    {
        base.Cast();
        print($"스킬 발동!! 이름 : {PieceName}");

        BattleHandler battleHandler = GameManager.Instance.battleHandler;

        Rullet rullet = battleHandler.rullets[0];
        List<RulletPiece> skillPieces = rullet.GetPieces();

        List<SkillPiece> lightningSkillPieces = new List<SkillPiece>();
        Dictionary<SkillPiece, int> lightningSkillIdxDic = new Dictionary<SkillPiece, int>();

        for (int i = 0; i < skillPieces.Count; i++)
        {
            if (skillPieces[i] == null) continue;

            if(skillPieces[i].PieceType.Equals(PieceType.SKILL))
            {
                if(skillPieces[i].comboType.Equals(PatternType.Diamonds) && skillPieces[i] != this)
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
        }

        Vector3 target = battleHandler.enemy.transform.position;
        target.y -= 0.7f;
        target.x += 0.5f;

        Anim_E_LightningRod lightningRodEffect = PoolManager.GetItem<Anim_E_LightningRod>();
        lightningRodEffect.transform.position = target;

        lightningRodEffect.Play(() => {

            // 번개 속성이 존재한다면
            if (result != null && CheckSilence())
            {
                result.Cast(onCastEnd);
                battleHandler.SetUseRulletPiece(lightningSkillIdxDic[result]);
            }
            else
            {
                onCastEnd?.Invoke();
            }
        });

        GameManager.Instance.battleHandler.enemy.GetDamage(Value);
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
    }
}
