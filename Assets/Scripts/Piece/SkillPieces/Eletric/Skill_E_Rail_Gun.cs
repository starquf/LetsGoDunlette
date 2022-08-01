using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_E_Rail_Gun : SkillPiece
{
    public override void Cast(LivingEntity target, Action onCastEnd = null) //사용 시 <sprite=11>를 4턴 받는다.
    {
        target.GetDamage(GetDamageCalc(Value), currentType);

        Vector3 dir = target.transform.position - Owner.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Vector3 rot = Quaternion.AngleAxis(angle, Vector3.forward).eulerAngles;

        animHandler.GetAnim(AnimName.Anim_ElecEffect04)
                .SetPosition(Owner.transform.position)
                .SetScale(1.5f)
                .SetRotation(rot)
                .Play(() =>
                {
                    Owner.GetComponent<LivingEntity>().cc.SetCC(CCType.Exhausted, 5);
                    animHandler.GetAnim(AnimName.ElecEffect05)
                            .SetPosition(Owner.transform.position)
                            .SetScale(1.5f)
                            .Play(() =>
                            {
                                onCastEnd?.Invoke();
                            });
                });
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(Value)}");
        return desInfos;
    }
}
