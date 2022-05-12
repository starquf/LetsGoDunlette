using System;
using Random = UnityEngine.Random;

public class MD_Skill : SkillPiece
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
            onCastSkill = MD_Hallucinations;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = MD_Scream;
            return pieceInfo[1];
        }
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void MD_Hallucinations(LivingEntity target, Action onCastEnd = null) //40% 확률로 침묵을 부여한다.
    {
        SetIndicator(owner.gameObject, "공격").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            target.GetDamage(15, this, owner);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                if (Random.Range(1, 100) <= 40)
                {
                    SetIndicator(owner.gameObject, "침묵부여").OnEndAction(() =>
                    {
                        target.cc.SetCC(CCType.Silence, 2);

                        animHandler.GetAnim(AnimName.M_Shield).SetPosition(GameManager.Instance.enemyEffectTrm.position)
                        .SetScale(2)
                        .Play(() =>
                        {
                            onCastEnd?.Invoke();
                        });
                    });
                }
                else
                {
                    onCastEnd?.Invoke();
                }
            });
        });
    }

    private void MD_Scream(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "공격").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });

            target.GetDamage(35, this, owner);
        });
    }


}
