using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_F_Ifrt : SkillPiece
{
    public override void Cast(LivingEntity target, Action onCastEnd = null) //�̹� ���� ���� �� ������ ������ ���� �귿�� �ִ� ��� ������ '��' �������� ���� ��Ų��.
    {
        target.GetDamage(GetDamageCalc(Value));
        onCastEnd?.Invoke();
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(Value)}");
        return desInfos;
    }
}
