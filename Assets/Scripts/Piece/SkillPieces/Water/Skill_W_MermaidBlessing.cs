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

        isTargeting = true;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc()}");

        return desInfos;
    }

    private int GetDamageCalc()
    {
        int attack = (int)(Owner.GetComponent<LivingEntity>().AttackPower * 0.3f);

        return attack;
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

        animHandler.GetAnim(AnimName.W_Splash01)
                    .SetPosition(target.transform.position)
                    .SetScale(1f)
                    .Play();

        target.GetDamage(GetDamageCalc(), currentType);

        yield return pTwoSecWait;

        for (int i = 0; i < skillPieces.Count; i++)
        {
            if (skillPieces[i] == null)
            {
                continue;
            }

            if (skillPieces[i].PieceType.Equals(PieceType.SKILL))
            {
                if (skillPieces[i].currentType != ElementalType.Water && (skillPieces[i] as SkillPiece).isPlayerSkill)
                {
                    int a = i;

                    animHandler.GetAnim(AnimName.W_Splash01)
                    .SetPosition(skillPieces[a].skillImg.transform.position)
                    .SetScale(0.5f)
                    .Play();

                    skillPieces[a].ChangeType(ElementalType.Water);
                    skillPieces[a].HighlightColor(0.4f);

                    animHandler.GetTextAnim()
                    .SetType(TextUpAnimType.Up)
                    .SetPosition(skillPieces[a].skillImg.transform.position)
                    .Play("속성 변경!");

                    yield return pTwoSecWait;
                }
            }
        }

        yield return pTwoSecWait;

        onCastEnd?.Invoke();
    }
}
