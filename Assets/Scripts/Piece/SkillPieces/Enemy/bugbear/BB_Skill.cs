using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BB_Skill : SkillPiece
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
            desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(pieceInfo[0].GetValue())}");

            onCastSkill = BB_Breaking_Armor;
            return pieceInfo[0];
        }
        else
        {
            desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(pieceInfo[1].GetValue())}");

            onCastSkill = BB_Strong_Attack;
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

    private void BB_Breaking_Armor(LivingEntity target, Action onCastEnd = null) //플레이어의 보호막을 전부 부순다.
    {
        if (target.HasShield())
        {
            SetIndicator(Owner.gameObject, "보호막 파괴").OnEndAction(() =>
            {
                target.RemoveShield();

                animHandler.GetAnim(AnimName.M_Scratch).SetPosition(GameManager.Instance.enemyEffectTrm.position)
                .SetScale(2)
                .Play(() =>
                {
                    SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
                    {
                        target.GetDamage(GetDamageCalc(pieceInfo[0].GetValue()));

                        animHandler.GetAnim(AnimName.M_Butt)
                            .SetPosition(GameManager.Instance.enemyEffectTrm.position)
                            .SetScale(2f)
                            .Play(() =>
                            {
                                onCastEnd?.Invoke();
                            });
                    });
                });
            });
        }
        else
        {
            SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
            {
                target.GetDamage(GetDamageCalc(pieceInfo[0].GetValue()));

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

    private void BB_Strong_Attack(LivingEntity target, Action onCastEnd = null) //스킬 사용 후 기절에 걸린다.
    {
        SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
        {
            target.GetDamage(GetDamageCalc(pieceInfo[1].GetValue()));

            animHandler.GetAnim(AnimName.M_Butt)
            .SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2f)
            .Play(() =>
            {
                SetIndicator(Owner.gameObject, "기절").OnEndAction(() =>
                {
                    Owner.GetComponent<EnemyHealth>().cc.SetCC(CCType.Stun, 1);
                    onCastEnd?.Invoke();
                });
            });
        });
    }


}
