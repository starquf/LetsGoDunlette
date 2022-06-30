using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_W_Check : SkillPiece
{
    [Header("½¯µå·®")]
    public int shieldValue = 40;

    protected override void Start()
    {
        base.Start();
    }
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());

        return desInfos;
    }
    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        target.GetDamage(GetDamageCalc(), currentType);

        animHandler.GetAnim(AnimName.W_Splash01)
            .SetPosition(skillIconImg.transform.position)
            .SetScale(0.5f)
            .Play(() =>
            {
                if (target.cc.IsCC())
                {
                    animHandler.GetAnim(AnimName.W_Splash01)
                    .SetPosition(skillIconImg.transform.position)
                    .SetScale(0.5f)
                    .Play(() =>
                    {
                        Owner.GetComponent<PlayerHealth>().AddShield(shieldValue);
                        onCastEnd?.Invoke();
                    });
                }
                else
                {
                    onCastEnd?.Invoke();
                }
            });
    }
}
