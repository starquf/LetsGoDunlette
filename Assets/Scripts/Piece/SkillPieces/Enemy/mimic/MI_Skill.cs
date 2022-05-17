using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MI_Skill : SkillPiece
{
    [Header("데미지 변수")]
    public int bittingDamage = 15;
    public int bumpDamage = 35;

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
            onCastSkill = MI_Biting;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = MI_Bump;
            return pieceInfo[1];
        }
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void MI_Biting(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            target.GetDamage(bittingDamage, this, Owner);

            animHandler.GetAnim(AnimName.M_Bite).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
           {
               SetIndicator(Owner.gameObject, "상처부여").OnEndAction(() =>
               {
                   target.cc.SetCC(CCType.Wound, 5, true);
                   onCastEnd?.Invoke();
               });
           });
        });


    }

    private void MI_Bump(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.7f, 0.15f);

            target.GetDamage(bumpDamage, this, Owner);

            animHandler.GetAnim(AnimName.M_Bite).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }


}
