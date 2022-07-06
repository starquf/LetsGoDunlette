using System;
using System.Collections.Generic;
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

    public override void Cast(LivingEntity target, Action onCastEnd = null) //������ ���� �ϳ��� ��� ����Ѵ�.
    {
        Rullet rullet = bh.mainRullet;
        List<RulletPiece> skillPieces = rullet.GetPieces();

        List<SkillPiece> selecetedSkillPieces = new List<SkillPiece>();

        for (int i = 0; i < skillPieces.Count; i++)
        {
            if (skillPieces[i] == null)
            {
                continue;
            }

            SkillPiece piece = skillPieces[i] as SkillPiece;

            if (piece.isPlayerSkill && piece != this)
            {
                selecetedSkillPieces.Add(piece);
            }
        }

        SkillPiece result = null;

        // ������ �����Ѵٸ�
        if (selecetedSkillPieces.Count > 0)
        {
            result = selecetedSkillPieces[Random.Range(0, selecetedSkillPieces.Count)];
            result.HighlightColor(0.4f);

            animHandler.GetTextAnim()
            .SetType(TextUpAnimType.Up)
            .SetPosition(result.skillIconImg.transform.position)
            .Play("���� ȿ���ߵ�!");
        }

        if (result != null)
        {
            bh.battleEvent.StartActionEvent(EventTimeSkill.WithSkill, result);

            bh.battleUtil.SetPieceToGraveyard(result.pieceIdx);

            bh.battleUtil.SetTimer(0.5f, () => { result.Cast(target, onCastEnd); });
        }
        else
        {
            onCastEnd?.Invoke();
        }
    }
}
