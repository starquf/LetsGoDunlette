using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_F_Last_Light : SkillPiece
{
    protected override void Start()
    {
        base.Start();
        bh = GameManager.Instance.battleHandler;
    }
    public override void Cast(LivingEntity target, Action onCastEnd = null) 
    {
        int damage = GetDamageCalc(Value);
        if(Owner.GetComponent<LivingEntity>().curHp <= 20)
        {
            damage += 12;
            animHandler.GetTextAnim()
                    .SetType(TextUpAnimType.Fixed)
                    .SetPosition(target.transform.position)
                    .Play("추가 피해!");
        }
        target.GetDamage(damage, currentType);
        animHandler.GetAnim(AnimName.LastLight)
                .SetPosition(Vector3.up * 3.3f)
                .SetScale(1f)
                .SetSortLayer(LayerMask.NameToLayer("Default"), 2)
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
