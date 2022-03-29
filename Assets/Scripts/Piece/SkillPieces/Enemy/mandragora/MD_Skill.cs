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

    private void MD_Hallucinations(LivingEntity target, Action onCastEnd = null) //40% Ȯ���� ħ���� �ο��Ѵ�.
    {
        SetIndicator(owner.gameObject, "����").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
            effect.transform.position = GameManager.Instance.enemyEffectTrm.position; effect.SetScale(2);

            target.GetDamage(15, this, owner);

            effect.Play(() =>
            {
                if (Random.Range(1, 100) <= 40)
                {
                    SetIndicator(owner.gameObject, "ħ���ο�").OnEnd(() =>
                    {
                        target.cc.SetCC(CCType.Silence, 2);

                        Anim_M_Recover effect1 = PoolManager.GetItem<Anim_M_Recover>();
                        effect1.transform.position = GameManager.Instance.enemyEffectTrm.position; effect.SetScale(2);
                        effect1.Play(() =>
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
        SetIndicator(owner.gameObject, "����").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
            effect.transform.position = GameManager.Instance.enemyEffectTrm.position; effect.SetScale(2);

            effect.Play(() =>
            {
                onCastEnd?.Invoke();
            });

            target.GetDamage(35, this, owner);
        });
    }


}
