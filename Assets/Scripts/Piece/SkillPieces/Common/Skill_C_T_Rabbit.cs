using System;
using System.Collections.Generic;

public class Skill_C_T_Rabbit : SkillPiece
{
    protected override void Start()
    {
        base.Start();
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Heal, Value.ToString());
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) //Èú	10
    {
        Owner.GetComponent<LivingEntity>().Heal(value);
        onCastEnd?.Invoke();
    }
}
