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

    public override void Cast(LivingEntity target, Action onCastEnd = null) //다음에 사용되는 <sprite=3>조각의 피해가 5증가한다.
    {
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
        GameManager.Instance.battleHandler.battleEvent.BookEvent(new SkillEvent(true,1,EventTimeSkill.WithSkill,(skill,action) => {
            if(skill.currentType == ElementalType.Fire)
            {
                skill.AddValue(value);
                animHandler.GetTextAnim()
                    .SetType(TextUpAnimType.Fixed)
                    .SetPosition(skill.transform.position)
                    .Play("강화!");
                animHandler.GetAnim(AnimName.Anim_FireEffect01)
                    .SetPosition(skill.transform.position)
                    .SetScale(2.5f)
                    .Play();
                GameManager.Instance.battleHandler.battleEvent.BookEvent(new NormalEvent(true, 0, (action) =>
                {
                    skill.MinusValue(value);
                    action?.Invoke();
                }, EventTime.EndOfTurn));
            }
            action?.Invoke();
        }));

        onCastEnd?.Invoke();
    }
}
