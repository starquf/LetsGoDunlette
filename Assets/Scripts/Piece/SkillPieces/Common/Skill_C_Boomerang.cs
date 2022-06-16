using System;
using System.Collections.Generic;

public class Skill_C_Boomerang : SkillPiece
{
    private int originValue = 0;

    protected override void Start()
    {
        base.Start();
        originValue = value;
    }
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());
        return desInfos;
    }

    public override void ResetPiece()
    {
        base.ResetPiece();
        value = originValue;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        target.GetDamage(GetDamageCalc());
        value++;
        animHandler.GetAnim(AnimName.E_ManaSphereHit)
            .SetScale(0.5f)
            .SetPosition(skillIconImg.transform.position)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
    }
}
