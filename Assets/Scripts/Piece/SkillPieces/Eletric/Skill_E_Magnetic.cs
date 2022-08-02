using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_E_Magnetic : SkillPiece
{
    public override void Cast(LivingEntity target, Action onCastEnd = null) //우측 부터 이 조각과 가장 가까운 자신의 조각을 사용한다.
    {
        target.GetDamage(Value, currentType);

        Rullet rullet = bh.mainRullet;
        List<RulletPiece> pieces = rullet.GetPieces();

        List<SkillPiece> nearSkillPieces = new List<SkillPiece>();

        // 스킬로 불러온 조각이라면
        if (!IsInRullet)
        {
            target.GetDamage(Value,currentType);
            onCastEnd?.Invoke();
            return;
        }

        int index;
        SkillPiece rulletPiece = GetNearPiece(out index);

        //존재한다면
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
            //오른쪽 체크
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
            
            //왼쪽 체크
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
