using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_W_Fishing : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
    }
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        return desInfos;
    }


    public override void Cast(LivingEntity target, Action onCastEnd = null) //무작위 조각 하나를 즉시 사용한다.
    {
        animHandler.GetAnim(AnimName.SkillEffect01)
        .SetPosition(Owner.transform.position)
        .SetScale(2f)
        .SetRotation(Vector3.forward * -90f)
        .Play(() =>
        {
            //onCastEnd?.Invoke();
        });
        List<RulletPiece> skillPieces = bh.mainRullet.GetPieces();
        SkillPiece result = null;
        int index = 0;
        if (skillPieces.Count > 0)
        {
            index = Random.Range(0, skillPieces.Count);
            result = skillPieces[index] as SkillPiece;
            if (result == null)
            {
                onCastEnd?.Invoke();
            }
            result.HighlightColor(0.4f);

            animHandler.GetTextAnim()
            .SetType(TextUpAnimType.Up)
            .SetPosition(result.skillIconImg.transform.position)
            .Play($"{result.PieceName} 월척!");
        }
        else
        {
            onCastEnd?.Invoke();
        }

        if (result.currentType == ElementalType.Monster)
        {
            result.ChoiceSkill();
            result.Cast(bh.player, onCastEnd);
            bh.battleUtil.SetPieceToGraveyard(index);
        }
        else
        {
            result.Cast(target, onCastEnd);
            bh.battleUtil.SetPieceToGraveyard(index);
        }

        print(8);
    }
}
