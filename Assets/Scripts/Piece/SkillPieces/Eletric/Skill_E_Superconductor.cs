using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_E_Superconductor : SkillPiece
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
        Owner.GetComponent<CrowdControl>().SetBuff(BuffType.Upgrade, 1);
        bh.battleEvent.BookEvent(new NormalEvent(true, 4, (action) =>
        {
            Owner.GetComponent<CrowdControl>().RemoveBuff(BuffType.Upgrade);
            action?.Invoke();
        }, EventTime.EndOfTurn));
        onCastEnd?.Invoke();
    }
}
