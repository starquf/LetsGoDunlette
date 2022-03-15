using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_F_ChainExplosion : SkillPiece
{

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        BattleHandler bh = GameManager.Instance.battleHandler;
        print($"��ų �ߵ�!! �̸� : {PieceName}");

        Vector3 targetPos = target.transform.position;

        Anim_F_ChainExplosion staticEffect = PoolManager.GetItem<Anim_F_ChainExplosion>();
        staticEffect.transform.position = targetPos;

        staticEffect.Play(() => {
            target.GetDamage(Value, patternType);
            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

            onCastEnd?.Invoke();
        });

        Action<RulletPiece> onNextAttack = result => { };
        int targetHp = target.curMaxHp;

        onNextAttack = result =>
        {
            if (result.GetComponent<SkillPiece>().isPlayerSkill && target.curMaxHp > targetHp)
            {
                target.GetDamage(Value, patternType);
                GameManager.Instance.cameraHandler.ShakeCamera(1.5f, 0.15f);

                Anim_F_ChainExplosionBonus bonusEffect = PoolManager.GetItem<Anim_F_ChainExplosionBonus>();
                bonusEffect.transform.position = targetPos;

                bonusEffect.Play();
            }

            // �ٷ� �����Ÿ� �̷���
            bh.battleEvent.onNextSkill -= onNextAttack;
        };

        // �̺�Ʈ�� �߰����ָ� ��
        bh.battleEvent.onNextSkill += onNextAttack;
    }
}
