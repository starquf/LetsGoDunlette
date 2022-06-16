using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_Plague : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
    }
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Wound, $"{Value}");
        return desInfos;
    }


    public override void Cast(LivingEntity target, Action onCastEnd = null) //����� ������ ��� ������ ����� <sprite=12>�� ���� ��ġ�� <sprite=12>�� �ο��Ѵ�.
    {

            target.cc.SetCC(CCType.Wound, Value);
            List<EnemyHealth> enemys = bh.enemys;
            for (int i = 0; i < enemys.Count; i++)
            {
                if (enemys[i] != target)
                {
                    enemys[i].cc.SetCC(CCType.Wound, target.cc.GetCCValue(CCType.Wound));
                }    
            }

            animHandler.GetAnim(AnimName.SkillEffect01)
            .SetPosition(Owner.transform.position)
            .SetScale(2f)
            .SetRotation(Vector3.forward * -90f)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
    }
}
