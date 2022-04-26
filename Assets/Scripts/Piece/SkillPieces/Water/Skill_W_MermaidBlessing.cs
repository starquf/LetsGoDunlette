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

        hasTarget = true;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        GameManager.Instance.battleHandler.battleUtil.StartCoroutine(Blessing(target, onCastEnd));
    }

    private IEnumerator Blessing(LivingEntity target, Action onCastEnd = null)
    {
        BattleHandler battleHandler = GameManager.Instance.battleHandler;

        Rullet rullet = battleHandler.mainRullet;
        List<RulletPiece> skillPieces = rullet.GetPieces();

        Anim_W_Splash splashEffect1 = PoolManager.GetItem<Anim_W_Splash>();
        splashEffect1.transform.position = target.transform.position;
        splashEffect1.SetScale(1f);

        splashEffect1.Play();

        target.GetDamage(Value, currentType);

        yield return pTwoSecWait;

        for (int i = 0; i < skillPieces.Count; i++)
        {
            if (skillPieces[i] == null) continue;

            if (skillPieces[i].PieceType.Equals(PieceType.SKILL))
            {
                if (skillPieces[i].currentType != ElementalType.Water && (skillPieces[i] as SkillPiece).isPlayerSkill)
                {
                    int a = i;

                    Anim_W_Splash splashEffect = PoolManager.GetItem<Anim_W_Splash>();
                    splashEffect.transform.position = skillPieces[a].skillImg.transform.position;
                    splashEffect.SetScale(0.5f);

                    skillPieces[a].ChangeType(ElementalType.Water);
                    skillPieces[a].HighlightColor(0.4f);

                    splashEffect.Play();

                    Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
                    textEffect.SetType(TextUpAnimType.Up);
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
