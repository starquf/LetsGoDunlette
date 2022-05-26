using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class FM_Skill : SkillPiece
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
            desInfos[0].SetInfo(DesIconType.Stun, $"{GetDamageCalc(pieceInfo[0].GetValue())}");

            onCastSkill = FM_Harpoon;
            return pieceInfo[0];
        }
        else
        {
            desInfos[0].SetInfo(DesIconType.Exhausted, $"{pieceInfo[1].GetValue()}턴");

            onCastSkill = FM_Splashing;
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

    private void FM_Harpoon(LivingEntity target, Action onCastEnd = null) //calcDamage	0 플레이어에게 보호막이 없다면 상처를 3턴 부여한다.
    {
        SetIndicator(Owner.gameObject, "작살").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

           if(target.GetShieldHp() <= 0)
            {
                target.cc.SetCC(CCType.Wound, 3);
            }

            target.GetDamage(GetDamageCalc(pieceInfo[0].GetValue()), this, Owner);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    private void FM_Splashing(LivingEntity target, Action onCastEnd = null) /// 피로	1/2턴
    {
        SetIndicator(Owner.gameObject, "물장구").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            target.cc.SetCC(CCType.Exhausted, pieceInfo[1].GetValue() + 1);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }


}
