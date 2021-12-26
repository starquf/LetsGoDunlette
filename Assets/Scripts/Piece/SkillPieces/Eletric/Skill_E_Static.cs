using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_E_Static : SkillPiece
{
    public GameObject staticEffectPrefab;
    public GameObject staticStunEffectPrefab;

    public override void Cast(Action onCastEnd = null)
    {
        BattleHandler bh = GameManager.Instance.battleHandler;
        base.Cast();
        print($"��ų �ߵ�!! �̸� : {PieceName}");

        Vector3 target = bh.enemy.transform.position;
        target.y -= 0.7f;

        Anim_E_Static staticEffect = PoolManager.GetItem<Anim_E_Static>();
        staticEffect.transform.position = target;

        staticEffect.Play(() => {
            bh.enemy.GetDamage(Value);
            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

            onCastEnd?.Invoke();
        });

        Action<RulletPiece> onNextTest = result => { };

        onNextTest = result =>
        {
            if (CheckSilence() && result.PieceType.Equals(PieceType.SKILL) && result.GetComponent<SkillPiece>().comboType.Equals(PatternType.Diamonds))
            {
                Anim_E_Static_Stun stunEffect = PoolManager.GetItem<Anim_E_Static_Stun>();
                stunEffect.transform.position = target;

                stunEffect.Play(()=>{
                    bh.enemy.cc.SetCC(CCType.Stun, 1);
                });
            }

            // �ٷ� �����Ÿ� �̷���
            bh.onNextAttack -= onNextTest;
        };

        // �̺�Ʈ�� �߰����ָ� ��
        bh.onNextAttack += onNextTest;
    }
}
