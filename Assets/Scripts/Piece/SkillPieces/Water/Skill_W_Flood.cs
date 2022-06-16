using System;
using System.Collections.Generic;

public class Skill_W_Flood : SkillPiece
{
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        animHandler.GetAnim(AnimName.E_ManaSphereHit)
            .SetScale(0.5f)
            .SetPosition(skillIconImg.transform.position)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });

        Flood();
    }

    private void Flood() //���Ӽ��� ������ ��� ������ �������� ����
    {
        List<RulletPiece> rulletPieces = bh.mainRullet.GetPieces();
        for (int i = 0; i < rulletPieces.Count; i++)
        {
            if (rulletPieces[i] != null)
            {
                if (rulletPieces[i].currentType != ElementalType.Water)
                {
                    bh.battleUtil.SetPieceToGraveyard(i);
                }
            }
        }
    }
}
