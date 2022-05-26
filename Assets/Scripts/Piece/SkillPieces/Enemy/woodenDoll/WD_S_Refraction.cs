using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WD_S_Refraction : SkillPiece
{
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Shield, $"{Value}");
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {

        SetIndicator(Owner.gameObject, "½¯µå").OnEndAction(() =>
        {
            Owner.GetComponent<LivingEntity>().AddShield(value);
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            animHandler.GetAnim(AnimName.M_Shield).SetPosition(Owner.transform.position)
            .SetScale(1)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }
}
