using System;
using System.Collections.Generic;

public class Skill_C_T_Coin : SkillPiece
{
    protected override void Start()
    {
        base.Start();
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) //"50 ��带 ��´�. ���� ���� �� �Ǵ� ��� �� �����ȴ�."
    {
        GameManager.Instance.AddGold(value);
        onCastEnd?.Invoke();
    }
}
