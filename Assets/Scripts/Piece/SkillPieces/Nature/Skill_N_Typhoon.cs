using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Skill_N_Typhoon : SkillPiece
{
    private BattleHandler bh = null;

    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;

        hasTarget = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        List<LivingEntity> targets = new List<LivingEntity>();

        EnemyHitEffectUtil.Typhoon(target.gameObject);

        if (target == bh.player)
        {
            targets.Add(target);
        }
        else 
        {
            targets = bh.battleUtil.DeepCopyEnemyList(bh.enemys);
        }

        for (int i = 0; i < targets.Count; i++)
        {
            Vector2 attackPos = targets[i].transform.position;

            Anim_N_Wind windEffect = PoolManager.GetItem<Anim_N_Wind>();
            windEffect.transform.position = attackPos;
            windEffect.SetScale(0.7f);

            windEffect.Play();

            targets[i].GetDamage(Value, currentType);

            if (targets.Count >= 2)
            {
                targets[i].GetDamage(50, currentType);
            }
        }

        Rullet rullet = bh.mainRullet;
        List<RulletPiece> pieces = rullet.GetPieces();

        for (int i = 0; i < 6; i++)
        {
            bh.battleUtil.SetPieceToGraveyard(i);
        }

        Vector2 effectPos = bh.mainRullet.transform.position;

        Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
        textEffect.SetType(TextUpAnimType.Up);
        textEffect.transform.position = effectPos;
        textEffect.SetScale(0.8f);
        textEffect.Play("��ǳ ȿ���ߵ�!");

        for (int i = 0; i < 7; i++)
        {
            Anim_N_Wind windEffect = PoolManager.GetItem<Anim_N_Wind>();
            windEffect.transform.position = effectPos + Random.insideUnitCircle * 2f;
            windEffect.SetScale(Random.Range(0.15f, 0.5f));

            windEffect.Play();
        }

        GameManager.Instance.cameraHandler.ShakeCamera(1.5f, 0.3f);
        GameManager.Instance.shakeHandler.ShakeBackCvsUI(1.5f, 0.3f);

        onCastEnd?.Invoke();
    }
}
