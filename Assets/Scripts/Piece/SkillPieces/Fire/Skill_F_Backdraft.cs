using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_F_Backdraft : SkillPiece
{
    protected override void Start()
    {
        base.Start();
        bh = GameManager.Instance.battleHandler;
    }
    public override void Cast(LivingEntity target, Action onCastEnd = null) 
    {
        target.GetDamage(GetDamageCalc(Value));

        int prevHp = Owner.GetComponent<LivingEntity>().curHp;
        GameManager.Instance.battleHandler.battleEvent.BookEvent(new NormalEvent(true, 1, (action) =>
        {
            if(Owner.GetComponent<LivingEntity>().curHp > prevHp)
            {
                //적 전체에게 추가 피해를 (<sprite=3>20)준다.
            }
            action?.Invoke();
        }, EventTime.EndOfTurn));
        
        onCastEnd?.Invoke();
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(Value)}");
        return desInfos;
    }
}
