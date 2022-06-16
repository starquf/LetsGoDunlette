using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Skill_E_High_Voltage : SkillPiece
{
    private Action<Action> highVolatge = null;
    private NormalEvent normalEvent = null;

    protected override void Start()
    {
        highVolatge = (a) => {
            if(Owner.GetComponent<LivingEntity>().GetShieldHp() <= 0)
            {
                for (int i = 0; i < bh.enemys.Count; i++)
                {
                    bh.enemys[i].cc.SetCC(CCType.Stun,1);
                }
            }
            a?.Invoke();
        };

        normalEvent = new NormalEvent(true, 1, highVolatge, EventTime.EndOfTurn);
    }
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Shield, $"{Value}");
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
            Owner.GetComponent<LivingEntity>().AddShield(Value);

            bh.battleEvent.BookEvent(normalEvent);

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
