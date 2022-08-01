using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_E_Magnetic : SkillPiece
{
    public override void Cast(LivingEntity target, Action onCastEnd = null) //���� ���� �� ������ ���� ����� �ڽ��� ������ ����Ѵ�.
    {
        target.GetDamage(Value, currentType);

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

        int index;
        SkillPiece rulletPiece = GetNearPiece(out index);

        //�����Ѵٸ�
        if (rulletPiece != null)
        {
            bh.battleEvent.StartActionEvent(EventTimeSkill.WithSkill, rulletPiece);
            rulletPiece.Cast(target, onCastEnd);
            bh.battleUtil.SetPieceToGraveyard(index);
        }
        else
        {
            onCastEnd?.Invoke();
        }
    }

    private SkillPiece GetNearPiece(out int idx)
    {
        Rullet rullet = bh.mainRullet;
        List<RulletPiece> pieces = rullet.GetPieces();
        RulletPiece nearPiece = null;

        SkillPiece piece = null;
        int index = pieceIdx;

        print(pieceIdx);

        for (int i = 1; i <= 2; i++)
        {
            //������ üũ
            index = (pieceIdx + i) % pieces.Count;
            nearPiece = pieces[index];
            
            if(nearPiece != this && nearPiece != null)
            {
                piece = nearPiece as SkillPiece;
                if(piece != null)
                {
                    if(piece.isPlayerSkill)
                    {
                        idx = index;
                        return piece;
                    }
                }
            }
            
            //���� üũ
            index = (pieceIdx - i);
            if(index <= -1)
            {
                index = 6;
            }
            nearPiece = pieces[index];

            if (nearPiece != this && nearPiece != null)
            {
                piece = nearPiece as SkillPiece;
                if (piece != null)
                {
                    if (piece.isPlayerSkill)
                    {
                        idx = index;
                        return piece;
                    }
                }
            }
        }

        index = (pieceIdx + 3) % pieces.Count;
        nearPiece = pieces[index];
        if (nearPiece != this)
        {
            piece = nearPiece as SkillPiece;
            if (piece != null)
            {
                if (piece.isPlayerSkill)
                {
                    idx = index;
                    return piece;
                }
            }
        }
        
        idx = 0;
        return null;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{Value}");
        return desInfos;
    }
}
