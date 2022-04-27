using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_C_Double_edged_sword : SkillPiece
{
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());

        return desInfos;
    }

    private int GetDamageCalc()
    {
        int attack = (int)(owner.GetComponent<LivingEntity>().AttackPower * 0.5f);

        return attack;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        PlayerHealth playerHealth = owner.GetComponent<PlayerHealth>();
        if (!playerHealth.HasShield())
        {
            playerHealth.GetDamage(2);
        }
        else
        {
            target.GetDamage(GetDamageCalc(), currentType);
        }


        Anim_C_SphereCast effect = PoolManager.GetItem<Anim_C_SphereCast>();
        effect.transform.position = skillImg.transform.position;
        effect.SetScale(0.5f);

        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }
}
