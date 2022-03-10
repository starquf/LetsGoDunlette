using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_W_MermaidBlessing : SkillPiece
{
    private readonly WaitForSeconds pTwoSecWait = new WaitForSeconds(0.2f);

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

        yield return pTwoSecWait;

        for (int i = 0; i < skillPieces.Count; i++)
        {
            if (skillPieces[i] == null) continue;

            if (skillPieces[i].PieceType.Equals(PieceType.SKILL))
            {
                if (skillPieces[i].currentType != PatternType.Spade && (skillPieces[i] as SkillPiece).isPlayerSkill)
                {
                    int a = i;

                    Anim_W_Splash splashEffect = PoolManager.GetItem<Anim_W_Splash>();
                    splashEffect.transform.position = skillPieces[a].skillImg.transform.position;
                    splashEffect.SetScale(0.5f);

                    skillPieces[a].ChangeType(PatternType.Spade);
                    skillPieces[a].HighlightColor(0.4f);

                    splashEffect.Play();

                    Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
                    textEffect.SetType(TextUpAnimType.Damage);
                    textEffect.transform.position = skillPieces[a].skillImg.transform.position;
                    textEffect.Play("속성 변경!");

                    yield return pTwoSecWait;
                }
            }
        }

        yield return pTwoSecWait;

        onCastEnd?.Invoke();
    }
}
