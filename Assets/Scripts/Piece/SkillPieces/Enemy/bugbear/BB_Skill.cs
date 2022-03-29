using System;
using System.Collections.Generic;
using UnityEngine;
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

    private void BB_Breaking_Armor(LivingEntity target, Action onCastEnd = null) //플레이어의 보호막을 전부 부순다.
    {
        if(target.HasShield())
        {
            SetIndicator(owner.gameObject, "보호막 파괴").OnEnd(() =>
            {
                target.RemoveAllShield();

                Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
                effect.transform.position = GameManager.Instance.enemyEffectTrm.position; effect.SetScale(2);
                effect.Play(() =>
                {
                    SetIndicator(owner.gameObject, "공격").OnEnd(() =>
                    {
                        target.GetDamage(20);

                        Anim_M_Butt effect = PoolManager.GetItem<Anim_M_Butt>();
                        effect.transform.position = GameManager.Instance.enemyEffectTrm.position; effect.SetScale(2);
                        effect.Play(() =>
                        {
                            onCastEnd?.Invoke();
                        });
                    });
                });
            });
        }
        else
        {
            SetIndicator(owner.gameObject, "공격").OnEnd(() =>
            {
                target.GetDamage(20);

                Anim_M_Butt effect = PoolManager.GetItem<Anim_M_Butt>();
                effect.transform.position = GameManager.Instance.enemyEffectTrm.position; effect.SetScale(2);
                effect.Play(() =>
                {
                    onCastEnd?.Invoke();
                });
            });
        }
    }

    private void BB_Strong_Attack(LivingEntity target, Action onCastEnd = null) //스킬 사용 후 기절에 걸린다.
    {
        SetIndicator(owner.gameObject, "공격").OnEnd(() =>
        {
            target.GetDamage(60);

            Anim_M_Butt effect = PoolManager.GetItem<Anim_M_Butt>();
            effect.transform.position = GameManager.Instance.enemyEffectTrm.position; effect.SetScale(2);
            effect.Play(() =>
            {
                SetIndicator(owner.gameObject, "기절").OnEnd(() =>
                {
                    owner.GetComponent<EnemyHealth>().cc.SetCC(CCType.Stun, 1);
                    onCastEnd?.Invoke();
                });
            });
        });
    }


}
