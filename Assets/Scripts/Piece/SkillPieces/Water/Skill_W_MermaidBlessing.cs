using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_W_MermaidBlessing : SkillPiece
{
    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);

    protected override void Start()
    {
        base.Start();

        hasTarget = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        StartCoroutine(Blessing(onCastEnd));
    }

    private IEnumerator Blessing(Action onCastEnd = null)
    {
        BattleHandler battleHandler = GameManager.Instance.battleHandler;

        Rullet rullet = battleHandler.mainRullet;
        List<RulletPiece> skillPieces = rullet.GetPieces();

        for (int i = 0; i < skillPieces.Count; i++)
        {
            if (skillPieces[i] == null) continue;

            if (skillPieces[i].PieceType.Equals(PieceType.SKILL))
            {
                if (skillPieces[i].currentType != PatternType.Spade && (skillPieces[i] as SkillPiece).isPlayerSkill)
                {
                    int a = i;

                    Anim_W_Splash splashEffect = PoolManager.GetItem<Anim_W_Splash>();
                    splashEffect.transform.position = skillPieces[i].skillImg.transform.position;

                    skillPieces[a].ChangeType(PatternType.Spade);

                    splashEffect.Play();

                    yield return pOneSecWait;
                }
            }
        }

        yield return pOneSecWait;

        onCastEnd?.Invoke();
    }
}
