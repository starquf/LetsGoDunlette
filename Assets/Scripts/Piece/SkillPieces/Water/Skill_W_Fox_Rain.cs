using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_W_Fox_Rain : SkillPiece
{
    protected override void Start()
    {
        base.Start();
        bh = GameManager.Instance.battleHandler;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) //∑Í∑øø° ¿˚ ¡∂∞¢¿Ã 4∞≥ ¿ÃªÛ¿œ ∞ÊøÏ ∑Í∑ø √ ±‚»≠
    {
        target.cc.SetCC(CCType.Exhausted, Value);

        List<RulletPiece> pieces = bh.mainRullet.GetPieces();
        int count = 0;
        foreach (var item in pieces)
        {
            if(item.patternType == ElementalType.Monster)
            {
                count++;
            }
        }

        if (count >= 4)
        {
            //bh.mainRullet.Reset();
        }
        //if(∑Í∑øø° ¿˚ ¡∂∞¢¿Ã 4∞≥ ¿ÃªÛ)
        //∑Í∑ø √ ±‚»≠

        onCastEnd?.Invoke();    
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Exhausted, $"{Value}");
        return desInfos;
    }
}
