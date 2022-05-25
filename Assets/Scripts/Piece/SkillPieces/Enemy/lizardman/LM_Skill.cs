using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LM_Skill : SkillPiece
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
            desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc(pieceInfo[0].GetValue()).ToString());

            onCastSkill = LM_Spearmanship;
            return pieceInfo[0];
        }
        else
        {
            desInfos[0].SetInfo(DesIconType.Shield, $"{pieceInfo[1].GetValue()}");

            onCastSkill = LM_Harden;
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

    private void LM_Spearmanship(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "°ø°Ý").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            animHandler.GetAnim(AnimName.M_Butt)
            .SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2f)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });

            target.GetDamage(GetDamageCalc(pieceInfo[0].GetValue()), this, Owner);
        });
    }

    private void LM_Harden(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "½¯µå").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            animHandler.GetAnim(AnimName.M_Shield)
            .SetPosition(Owner.transform.position)
            .SetScale(1)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });

            Owner.GetComponent<EnemyHealth>().AddShield(pieceInfo[1].GetValue());
        });
    }
}
