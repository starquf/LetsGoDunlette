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
        target.GetDamage(GetDamageCalc(Value), currentType);

        int prevHp = Owner.GetComponent<LivingEntity>().curShieldAndHP;
        GameManager.Instance.battleHandler.battleEvent.BookEvent(new NormalEvent(true, 2, (action) =>
        {
            if(Owner.GetComponent<LivingEntity>().curShieldAndHP > prevHp)
            {
                //적 전체에게 추가 피해를 (<sprite=3>20)준다.
                foreach (var item in GameManager.Instance.battleHandler.enemys)
                {
                    item.GetDamage(20,currentType);
                    animHandler.GetAnim(AnimName.Anim_FireEffect04)
                            .SetPosition(item.transform.position)
                            .SetScale(1f)
                            .Play();
                }
            }
            action?.Invoke();
        }, EventTime.EndOfTurn));

        animHandler.GetAnim(AnimName.Anim_FireEffect02)
                .SetPosition(target.transform.position)
                .SetScale(1f)
                .Play(()=>
                {
                    onCastEnd?.Invoke();
                });

    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(Value)}");
        return desInfos;
    }
}
