using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_F_Ashes : SkillPiece
{
    public override void Cast(LivingEntity target, Action onCastEnd = null) //�̹� ���� ���� �� ������ ������ ���� �귿�� �ִ� ��� ������ '��' �������� ���� ��Ų��.
    {
        target.GetDamage(Value, currentType);
        animHandler.GetAnim(AnimName.SkillEffect06)
                .SetPosition(target.transform.position)
                .SetScale(1.2f)
                .Play(() =>
                {
                    onCastEnd?.Invoke();
                });
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{Value}");
        return desInfos;
    }
}
