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


    public override void Cast(LivingEntity target, Action onCastEnd = null) //������ ���� �ϳ��� ��� ����Ѵ�.
    {
        var skillPieces = GameManager.Instance.inventoryHandler.skills;
        SkillPiece result = null;
        int index = 0;
        if (skillPieces.Count > 0)
        {
            index = Random.Range(0, skillPieces.Count);
            result = skillPieces[index];
            if (result == null)
            {
                onCastEnd?.Invoke();
            }
        }
        else
        {
            onCastEnd?.Invoke();
        }

        print(result.name);

        if (!(result.currentType == ElementalType.Monster) && result.isTargeting == true)
        {
            if(result.IsInRullet)
            {
                result.Cast(target, onCastEnd);
                bh.battleUtil.SetPieceToGraveyard(index);
            }
        }
        else
        {
            onCastEnd?.Invoke();
        }
    }
}
