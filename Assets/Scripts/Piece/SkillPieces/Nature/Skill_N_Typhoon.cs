using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        List<EnemyHealth> enemys = bh.battleUtil.DeepCopyList(bh.enemys);

        for (int i = 0; i < enemys.Count; i++)
        {
            Vector2 attackPos = enemys[i].transform.position;

            Anim_N_Wind windEffect = PoolManager.GetItem<Anim_N_Wind>();
            windEffect.transform.position = attackPos;
            windEffect.SetScale(0.7f);

            windEffect.Play();

            enemys[i].GetDamage(Value, currentType);

            if (enemys.Count >= 2)
            {
                enemys[i].GetDamage(50, currentType);
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
        textEffect.SetType(TextUpAnimType.Damage);
        textEffect.transform.position = effectPos;
        textEffect.SetScale(0.8f);
        textEffect.Play("태풍 효과발동!");

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
