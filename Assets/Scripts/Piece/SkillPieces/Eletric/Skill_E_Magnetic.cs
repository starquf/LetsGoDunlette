using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_E_Magnetic : SkillPiece
{
    public override void Cast(LivingEntity target, Action onCastEnd = null) //���� ���� �� ������ ���� ����� �ڽ��� ������ ����Ѵ�.
    {
        target.GetDamage(Value);

        Rullet rullet = bh.mainRullet;
        List<RulletPiece> pieces = rullet.GetPieces();

        List<SkillPiece> nearSkillPieces = new List<SkillPiece>();

        // ��ų�� �ҷ��� �����̶��
        if (!IsInRullet)
        {
            target.GetDamage(Value,currentType);
            onCastEnd?.Invoke();
            return;
        }

        SkillPiece rulletPiece = GetNearPiece();

        //�����Ѵٸ�
        if (rulletPiece != null)
        {
            bh.battleEvent.StartActionEvent(EventTimeSkill.WithSkill, rulletPiece);
            rulletPiece.Cast(target, onCastEnd);
            bh.battleUtil.SetPieceToGraveyard(rulletPiece);
        }
        else
        {
            onCastEnd?.Invoke();
        }
    }

    private SkillPiece GetNearPiece()
    {
        Rullet rullet = bh.mainRullet;
        List<RulletPiece> pieces = rullet.GetPieces();
        RulletPiece nearPiece = null;

        SkillPiece piece = null;
        int index = pieceIdx;

        for (int i = 0; i < 2; i++)
        {
            //������ üũ
            index = (pieceIdx + i) % pieces.Count;
            nearPiece = pieces[index];
            
            if(nearPiece != this)
            {
                piece = nearPiece as SkillPiece;
                if(piece.isPlayerSkill)
                {
                    return piece;
                }
            }
            
            //���� üũ
            index = (pieceIdx - i);
            if(index <= -1 )
            {
                index = 6;
            }
            nearPiece = pieces[index];

            if (nearPiece != this)
            {
                piece = nearPiece as SkillPiece;
                if (piece.isPlayerSkill)
                {
                    return piece;
                }
            }
        }


        nearPiece = pieces[(pieceIdx + 3) % pieces.Count];
        if (nearPiece != this)
        {
            piece = nearPiece as SkillPiece;
            if (piece.isPlayerSkill)
            {
                return piece;
            }
        }

        return piece;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{Value}");
        return desInfos;
    }
}
