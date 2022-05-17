using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_F_Arson : SkillPiece
{
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc()}");

        return desInfos;
    }

    private int GetDamageCalc()
    {
        int attack = (int)(Owner.GetComponent<LivingEntity>().AttackPower * 0.5f);

        return attack;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        Vector3 targetPos = target.transform.position;

        animHandler.GetAnim(AnimName.F_Arson)
                .SetPosition(targetPos)
                .SetRotation(Vector3.forward * 90f)
                .Play(() =>
                {
                    target.GetDamage(GetDamageCalc(), currentType);
                    GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

                    onCastEnd?.Invoke();
                });
    }
}
