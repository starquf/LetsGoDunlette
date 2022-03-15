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

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        if (Random.Range(1, 100) <= value)
        {
            BB_Breaking_Armor(target, onCastEnd);
        }
        else
        {
            BB_Strong_Attack(target, onCastEnd);
        }
    }

    private void BB_Breaking_Armor(LivingEntity target, Action onCastEnd = null) //�÷��̾��� ��ȣ���� ���� �μ���.
    {
        if(target.HasShield())
        {
            SetIndicator(owner.gameObject, "��ȣ�� �ı�").OnEnd(() =>
            {
                target.RemoveAllShield();

                Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
                effect.transform.position = owner.transform.position;
                effect.Play(() =>
                {
                    SetIndicator(owner.gameObject, "����").OnEnd(() =>
                    {
                        target.GetDamage(20);

                        Anim_M_Butt effect = PoolManager.GetItem<Anim_M_Butt>();
                        effect.transform.position = owner.transform.position;
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
            SetIndicator(owner.gameObject, "����").OnEnd(() =>
            {
                target.GetDamage(20);

                Anim_M_Butt effect = PoolManager.GetItem<Anim_M_Butt>();
                effect.transform.position = owner.transform.position;
                effect.Play(() =>
                {
                    onCastEnd?.Invoke();
                });
            });
        }
    }

    private void BB_Strong_Attack(LivingEntity target, Action onCastEnd = null) //��ų ��� �� ������ �ɸ���.
    {
        SetIndicator(owner.gameObject, "����").OnEnd(() =>
        {
            target.GetDamage(80);

            Anim_M_Butt effect = PoolManager.GetItem<Anim_M_Butt>();
            effect.transform.position = owner.transform.position;
            effect.Play(() =>
            {
                SetIndicator(owner.gameObject, "����").OnEnd(() =>
                {
                    owner.GetComponent<EnemyHealth>().cc.SetCC(CCType.Stun, 1);
                    onCastEnd?.Invoke();
                });
            });
        });
    }


}
