using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_C_Sign : SkillPiece
{
    public override void Cast(Action onCastEnd = null)
    {
        BattleHandler bh = GameManager.Instance.battleHandler;

        Anim_Contract contractAnim = PoolManager.GetItem<Anim_Contract>();
        contractAnim.transform.position = bh.player.transform.position;

        contractAnim.Play(() => {
            onCastEnd?.Invoke();
        });

        /* 
         
        // Ŭ���� ����
        Action<RulletPiece> onNextTest = result => { };

        onNextTest = result =>
        {
            if (result.PieceType.Equals(PieceType.SKILL))
            {
                print("�ȳ�");
                bh.enemy.GetDamage(10);
                GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
            }
            
            // �ٷ� �����Ÿ� �̷���
            bh.onNextAttack -= onNextTest;
        };

        // �̺�Ʈ�� �߰����ָ� ��
        bh.onNextAttack += onNextTest;

        */

        bh.SetContract(Value, 3);
    }
}
