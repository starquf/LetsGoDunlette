using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_F_ChainExplosion : SkillPiece
{

    public override void Cast(Action onCastEnd = null)
    {
        BattleHandler bh = GameManager.Instance.battleHandler;
        base.Cast();
        print($"��ų �ߵ�!! �̸� : {PieceName}");

        Vector3 target = bh.enemy.transform.position;

        Anim_F_ChainExplosion staticEffect = PoolManager.GetItem<Anim_F_ChainExplosion>();
        staticEffect.transform.position = target;

        staticEffect.Play(() => {
            bh.enemy.GetDamage(Value);
            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

            onCastEnd?.Invoke();
        });

        Action<RulletPiece> onNextTest = result => { };

        onNextTest = result =>
        {
            if (!CheckSilence() && result.PieceType.Equals(PieceType.SKILL) && result.GetComponent<SkillPiece>().isPlayerSkill)
            {
                Anim_F_ChainExplosionBonus bonusEffect = PoolManager.GetItem<Anim_F_ChainExplosionBonus>();
                bonusEffect.transform.position = target;

                bonusEffect.Play(() => {
                    bh.enemy.GetDamage(Value);
                    GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
                });
            }

            // �ٷ� �����Ÿ� �̷���
            bh.onNextAttack -= onNextTest;
        };

        // �̺�Ʈ�� �߰����ָ� ��
        bh.onNextAttack += onNextTest;
    }
}
