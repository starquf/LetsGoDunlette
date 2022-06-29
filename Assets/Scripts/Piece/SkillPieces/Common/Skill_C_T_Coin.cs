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

    public override void Cast(LivingEntity target, Action onCastEnd = null) //"50 골드를 얻는다. 전투 종료 시 또는 사용 후 삭제된다."
    {
        GameManager.Instance.AddGold(value);
        onCastEnd?.Invoke();
    }
}
