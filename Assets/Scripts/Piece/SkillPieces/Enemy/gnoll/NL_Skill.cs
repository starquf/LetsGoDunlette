using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class NL_Skill : SkillPiece
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
            desInfos[0].SetInfo(DesIconType.Wound, $"{pieceInfo[0].GetValue()}");

            onCastSkill = NL_Poison_Dagger;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = NL_Mark;

            desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(pieceInfo[1].GetValue())}");
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

    private void NL_Poison_Dagger(LivingEntity target, Action onCastEnd = null) //��ó�� �ο��ؼ� 2�� ���� 10�� ���ظ� ������..
    {
        SetIndicator(Owner.gameObject, "��ó �ο�").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            target.cc.SetCC(CCType.Wound, pieceInfo[0].GetValue(), true);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    private void NL_Mark(LivingEntity target, Action onCastEnd = null) //���� ��� ������ ���ذ� 5 ����Ѵ�. ��ó ����
    {
        SetIndicator(Owner.gameObject, "��ɲ��� ǥ��").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            target.GetDamage(GetDamageCalc(pieceInfo[1].GetValue()), this, Owner);

            if (target.cc.IsCC(CCType.Wound))
            {
                int ccValue = target.cc.GetCCValue(CCType.Wound);
                target.cc.IncreaseCCTurn(CCType.Wound, ccValue);
            }

            animHandler.GetAnim(AnimName.M_Wisp).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }


}
