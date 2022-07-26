using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_FireWood : SkillPiece
{

    protected override void Start()
    {
        base.Start();
        bh = GameManager.Instance.battleHandler;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) //������ ���Ǵ� <sprite=3>������ ���ذ� 5�����Ѵ�.
    {
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
        GameManager.Instance.battleHandler.battleEvent.BookEvent(new SkillEvent(true,1,EventTimeSkill.WithSkill,(skill,action) => {
            if(skill.currentType == ElementalType.Fire)
            {
                print("�ߵ�");       
            }
            action?.Invoke();
        }
        ));

        onCastEnd?.Invoke();
    }
}
