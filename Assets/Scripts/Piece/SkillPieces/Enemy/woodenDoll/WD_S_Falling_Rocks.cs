using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WD_S_Falling_Rocks : SkillPiece
{
    private int percent = 30;

    protected override void Awake()
    {
        base.Awake();

        isPlayerSkill = false;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(Value)}");
        desInfos[1].SetInfo(DesIconType.Stun, $"{percent}%");
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
        {
            target.GetDamage(GetDamageCalc(Value), this, Owner);
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            if (Random.Range(0, 100) < percent)
            {
                animHandler.GetTextAnim()
                .SetType(TextUpAnimType.Fixed)
                .SetPosition(target.transform.position)
                .SetScale(0.7f)
                .Play("기절!");

                target.cc.SetCC(CCType.Stun, 1);
            }

            animHandler.GetAnim(AnimName.M_Butt).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }
}
