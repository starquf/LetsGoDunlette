using System;
using Random = UnityEngine.Random;

public class GA_Skill : SkillPiece
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
            onCastSkill = GA_Solidification;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = GA_Press;
            return pieceInfo[1];
        }
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void GA_Solidification(LivingEntity target, Action onCastEnd = null) // 전투가 종료될 때까지 매턴 5의 보호막을 얻는다. 중첩가능
    {
        SetIndicator(owner.gameObject, "보호막 추가").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            Anim_M_Shield hitEffect = PoolManager.GetItem<Anim_M_Shield>();
            hitEffect.transform.position = owner.transform.position;

            owner.GetComponent<GA_Solidification>().shieldValue += 5;

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    private void GA_Press(LivingEntity target, Action onCastEnd = null) //현재 자신의 보호막만큼 플레이어에게 추가 피해를 준다.
    {
        SetIndicator(owner.gameObject, "공격").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            target.GetDamage(5, this, owner);

            Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
            effect.transform.position = GameManager.Instance.enemyEffectTrm.position; effect.SetScale(2);

            effect.Play(() =>
            {
                int curShield = owner.GetComponent<EnemyHealth>().GetShieldHp();
                if (curShield > 0)
                {
                    SetIndicator(owner.gameObject, $"추가 피해 +{curShield}").OnEnd(() =>
                    {
                        target.GetDamage(curShield, this, owner);

                        Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
                        effect.transform.position = GameManager.Instance.enemyEffectTrm.position; effect.SetScale(2);

                        effect.Play(() =>
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


}
