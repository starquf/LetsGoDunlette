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

    private void AG_Diving(LivingEntity target, Action onCastEnd = null) //2�ϰ� ħ���� �ް� �ƹ� �������� ���� �ʴ´�. 2���� ������ �÷��̾�� 60��ŭ ���ظ� ������.
    {
        GameManager.Instance.battleHandler.battleEvent.BookEvent(new BookedEventInfo(() =>
        {
            SetIndicator(owner.gameObject, "����").OnEnd(() =>
            {
                target.GetDamage(60);

                Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
                effect.transform.position = owner.transform.position;
                effect.Play(() =>
                {
                });
            });
        }, 3));

        SetIndicator(owner.gameObject, "ħ��").OnEnd(() =>
        {
            owner.GetComponent<EnemyHealth>().cc.SetCC(CCType.Silence, 3);

            Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
            effect.transform.position = owner.transform.position;
            effect.Play(() =>
            {
                SetIndicator(owner.gameObject, "����").OnEnd(() =>
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

    private void AG_Crocodile_Bird(LivingEntity target, Action onCastEnd = null) //�ڽ��� ü���� 40��ŭ ȸ���Ѵ�.
    {
        SetIndicator(owner.gameObject, "ȸ��").OnEnd(() =>
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
