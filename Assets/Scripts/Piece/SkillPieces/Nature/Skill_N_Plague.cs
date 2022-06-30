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
        animHandler.GetAnim(AnimName.N_PoisionCloud)
        .SetPosition(target.transform.position)
        .SetScale(1f)
        .Play(() =>
        {
            List<EnemyHealth> enemys = bh.enemys;
            if(enemys.Count !=1)
            {
                for (int i = 0; i < enemys.Count; i++)
                {
                    int idx = i;
                    if (enemys[i] != target)
                    {
                        animHandler.GetAnim(AnimName.N_PoisionCloud)
                        .SetPosition(target.transform.position)
                        .SetScale(1f)
                        .Play();
                        enemys[i].cc.SetCC(CCType.Wound, target.cc.GetCCValue(CCType.Wound));
                        if (i == enemys.Count - 1)
                        {
                            onCastEnd?.Invoke();
                        }
                    }
                }
            }
            else
            {
                onCastEnd?.Invoke();
            }
        });
        target.cc.SetCC(CCType.Wound, Value);

    }
}
