using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_F_Furnace : SkillPiece
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
    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        Owner.GetComponent<CrowdControl>().IncreaseBuff(BuffType.Upgrade, 1);
        bh.battleEvent.BookEvent(new NormalEvent(true, 4, (action) =>
        {
            print("Ω√¿€");
            Owner.GetComponent<CrowdControl>().DecreaseBuff(BuffType.Upgrade, 1);
            action?.Invoke();
        }, EventTime.EndOfTurn));
        animHandler.GetAnim(AnimName.SmokeEffect06)
                .SetPosition(Owner.transform.position)
                .SetScale(2f)
                .SetColor(new Color(0.3f, 0.2f, 0.2f))
                .Play();
        animHandler.GetAnim(AnimName.BuffEffect04)
                .SetPosition(Owner.transform.position)
                .SetScale(1.5f)
                .Play(() =>
                {
                    onCastEnd?.Invoke();
                });
    }
}

