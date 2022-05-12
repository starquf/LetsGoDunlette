using System;
using Random = UnityEngine.Random;

public class NL_Skill : SkillPiece
{
    private int addAdditionalDamage = 0;
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
            onCastSkill = NL_Poison_Dagger;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = NL_Mark;
            return pieceInfo[1];
        }
    }


    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void NL_Poison_Dagger(LivingEntity target, Action onCastEnd = null) //상처를 부여해서 2턴 동안 10의 피해를 입힌다..
    {
        SetIndicator(owner.gameObject, "공격").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            target.GetDamage(20 + addAdditionalDamage, this, owner);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                SetIndicator(owner.gameObject, "상처부여").OnEndAction(() =>
                {
                    target.cc.SetCC(CCType.Wound, 2, true);
                    onCastEnd?.Invoke();
                });
            });
        });
    }

    private void NL_Mark(LivingEntity target, Action onCastEnd = null) //놀의 모든 공격의 피해가 5 상승한다. 상처 제외
    {
        SetIndicator(owner.gameObject, "강화").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });

            addAdditionalDamage += 5;

            for (int i = 0; i < owner.skills.Count; i++)
            {
                NL_Attack skill = owner.skills[i].GetComponent<NL_Attack>();
                if (skill != null)
                {
                    skill.AddValue(5);

                    // break; // 1개 라고 가정함
                }
            }
        });
    }


}
