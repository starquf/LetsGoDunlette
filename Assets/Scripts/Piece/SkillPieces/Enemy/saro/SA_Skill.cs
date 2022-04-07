using System;
using Random = UnityEngine.Random;

public class SA_Skill : SkillPiece
{
    public bool sacrificed = false;
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override PieceInfo ChoiceSkill()
    {
        base.ChoiceSkill();
        onCastSkill = SR_CutOff;
        return pieceInfo[0];
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void SR_CutOff(LivingEntity target, Action onCastEnd = null) // 플레이어에게 30의 피해를 입힌다. //가르가 죽으면 절단의 데미지가 30만큼 증가한다.
    {
        SetIndicator(owner.gameObject, "공격").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            Anim_M_Bite hitEffect = PoolManager.GetItem<Anim_M_Bite>();
            hitEffect.transform.position = GameManager.Instance.enemyEffectTrm.position; hitEffect.SetScale(2);
            if(sacrificed)
            {
                target.GetDamage(30 + 30, this, owner);
            }
            else
            {
                target.GetDamage(30, this, owner);
            }
            hitEffect.Play(() =>
            {
                SetIndicator(owner.gameObject, "상처부여").OnEnd(() =>
                {
                    target.cc.SetCC(CCType.Wound, 5);
                    onCastEnd?.Invoke();
                });
            });
        });
    }

    private void SR_Sacrifice1(LivingEntity target, Action onCastEnd = null) //가르가 죽으면 절단의 데미지가 30만큼 증가한다. 1회성
    {
        SetIndicator(owner.gameObject, "스킬 강화").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.7f, 0.15f);

            Anim_M_Recover hitEffect = PoolManager.GetItem<Anim_M_Recover>();
            hitEffect.transform.position = owner.transform.position;

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    private void SR_Sacrifice2(LivingEntity target, Action onCastEnd = null) //든암이 죽으면 보호막을 300 획득한다.
    {
        SetIndicator(owner.gameObject, "보호막").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.7f, 0.15f);

            Anim_M_Recover hitEffect = PoolManager.GetItem<Anim_M_Recover>();
            hitEffect.transform.position = owner.transform.position;

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }


}
