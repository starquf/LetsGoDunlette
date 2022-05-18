using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class NSL_Skill : SkillPiece
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
            ResetDesInfo();
            desInfos[0].SetInfo(DesIconType.Heal, $"{pieceInfo[0].GetValue()}");

            onCastSkill = NSl_Recover;
            return pieceInfo[0];
        }
        else
        {
            ResetDesInfo();
            desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc(pieceInfo[1].GetValue()).ToString());

            onCastSkill = NSL_Bounce;
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

    private void NSl_Recover(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "ȸ��").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            animHandler.GetAnim(AnimName.M_Recover).SetPosition(Owner.transform.position)
            .SetScale(1)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });

            Owner.GetComponent<EnemyHealth>().Heal(pieceInfo[0].GetValue());
        });
    }

    private void NSL_Bounce(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "����").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            target.GetDamage(GetDamageCalc(pieceInfo[1].GetValue()), this, Owner);

            animHandler.GetAnim(AnimName.M_Butt)
            .SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2f)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }


}
