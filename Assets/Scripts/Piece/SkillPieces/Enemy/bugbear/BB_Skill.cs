using System;
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
            onCastSkill = BB_Breaking_Armor;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = BB_Strong_Attack;
            return pieceInfo[1];
        }
    }


    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void BB_Breaking_Armor(LivingEntity target, Action onCastEnd = null) //�÷��̾��� ��ȣ���� ���� �μ���.
    {
        if (target.HasShield())
        {
            SetIndicator(Owner.gameObject, "��ȣ�� �ı�").OnEndAction(() =>
            {
                target.RemoveShield();

                animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
                .SetScale(2)
                .Play(() =>
                {
                    SetIndicator(Owner.gameObject, "����").OnEndAction(() =>
                    {
                        target.GetDamage(20);

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
            SetIndicator(Owner.gameObject, "����").OnEndAction(() =>
            {
                target.GetDamage(20);

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

    private void BB_Strong_Attack(LivingEntity target, Action onCastEnd = null) //��ų ��� �� ������ �ɸ���.
    {
        SetIndicator(Owner.gameObject, "����").OnEndAction(() =>
        {
            target.GetDamage(60);

            animHandler.GetAnim(AnimName.M_Butt)
            .SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2f)
            .Play(() =>
            {
                SetIndicator(Owner.gameObject, "����").OnEndAction(() =>
                {
                    Owner.GetComponent<EnemyHealth>().cc.SetCC(CCType.Stun, 1);
                    onCastEnd?.Invoke();
                });
            });
        });
    }


}
