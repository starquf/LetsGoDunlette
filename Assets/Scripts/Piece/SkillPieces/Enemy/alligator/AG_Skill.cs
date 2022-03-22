using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AG_Skill : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        if (Random.Range(0, 100) <= value)
        {
            AG_Diving(target, onCastEnd);
        }
        else
        {
            AG_Crocodile_Bird(target, onCastEnd);
        }
    }

    private void AG_Diving(LivingEntity target, Action onCastEnd = null) //2턴간 침묵을 받고 아무 데미지도 받지 않는다. 2턴이 지나면 플레이어에게 60만큼 피해를 입힌다.
    {
        GameManager.Instance.battleHandler.battleEvent.BookEvent(new BookedEventInfo(() =>
        {
            SetIndicator(owner.gameObject, "공격").OnEnd(() =>
            {
                target.GetDamage(60);

                Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
                effect.transform.position = owner.transform.position;
                effect.Play(() =>
                {
                });
            });
        }, 3));

        SetIndicator(owner.gameObject, "침묵").OnEnd(() =>
        {
            owner.GetComponent<EnemyHealth>().cc.SetCC(CCType.Silence, 3);

            Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
            effect.transform.position = owner.transform.position;
            effect.Play(() =>
            {
                SetIndicator(owner.gameObject, "무적").OnEnd(() =>
                {
                    owner.GetComponent<EnemyHealth>().cc.SetCC(CCType.Invincibility, 3);

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

    private void AG_Crocodile_Bird(LivingEntity target, Action onCastEnd = null) //자신의 체력을 40만큼 회복한다.
    {
        SetIndicator(owner.gameObject, "회복").OnEnd(() =>
        {
            owner.GetComponent<EnemyHealth>().Heal(30);

            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            Anim_M_Recover effect = PoolManager.GetItem<Anim_M_Recover>();
            effect.transform.position = owner.transform.position;

            effect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }


}
