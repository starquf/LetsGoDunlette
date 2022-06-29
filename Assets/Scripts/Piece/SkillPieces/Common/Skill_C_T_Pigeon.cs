using System;
using System.Collections.Generic;

public class Skill_C_T_Pigeon : SkillPiece
{
    protected override void Start()
    {
        base.Start();
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Silence, Value.ToString());
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) //Ä§¹¬	3ÅÏ
    {
        target.cc.SetCC(CCType.Silence, value + 1);
        onCastEnd?.Invoke();
    }
}
