using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DP_Skill : SkillPiece
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
            onCastSkill = DP_Duty;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = DP_Poke;
            return pieceInfo[1];
        }
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void DP_Duty(LivingEntity target, Action onCastEnd = null)
    {
        EnemyHealth boss = null;

        List<EnemyHealth> enemys = GameManager.Instance.battleHandler.enemys;

        for (int i = 0; i < enemys.Count; i++)
        {
            var health = enemys[i];

            if (health.isBoss)
            {
                boss = health;
                // queen 임.
                break; // 여왕은 1명이라는 가정
            }
        }

        if (boss == null)
        {
            Debug.LogError("보스가 없음");
        }

        SetIndicator(owner.gameObject, "희생").OnEnd(() =>
        {
            owner.GetComponent<EnemyHealth>().GetDamageIgnoreShild(40);

            Anim_M_Butt effect = PoolManager.GetItem<Anim_M_Butt>();
            effect.transform.position = owner.transform.position;
            effect.SetScale(1f);
            effect.Play();

            SetIndicator(boss.gameObject, "회복").OnEnd(() =>
            {
                GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

                boss.Heal(30);

                Anim_M_Recover healEffect = PoolManager.GetItem<Anim_M_Recover>();
                healEffect.transform.position = boss.transform.position;
                healEffect.Play(() =>
                {
                    onCastEnd?.Invoke();
                });
            });
        });
        //나중에 힐 이펙트가 여왕한테 가야함   
    }

    private void DP_Poke(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "공격").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            Anim_M_Sword hitEffect = PoolManager.GetItem<Anim_M_Sword>();
            hitEffect.transform.position = GameManager.Instance.enemyEffectTrm.position; hitEffect.SetScale(2);

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
            target.GetDamage(15, this, owner);
        });
    }


}
