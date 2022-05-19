using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MI_Skill : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override PieceInfo ChoiceSkill()
    {
        base.ChoiceSkill();
        if (Random.Range(0, 100) <= value)
        {
            desInfos[0].SetInfo(DesIconType.Wound, $"{pieceInfo[0].GetValue()}턴");

            onCastSkill = MI_Biting;
            return pieceInfo[0];
        }
        else
        {
            desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(pieceInfo[1].GetValue())}");

            onCastSkill = MI_Bump;
            return pieceInfo[1];
        }
    }
    public override List<DesIconInfo> GetDesIconInfo()
    {
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void MI_Biting(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "상처 부여").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            target.cc.SetCC(CCType.Wound, pieceInfo[0].GetValue(), true);

            animHandler.GetAnim(AnimName.M_Bite).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
           {
                   onCastEnd?.Invoke();
           });
        });


    }

    private void MI_Bump(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.7f, 0.15f);

            target.GetDamage(GetDamageCalc(pieceInfo[1].GetValue()), this, Owner);

            animHandler.GetAnim(AnimName.M_Bite).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }


}
