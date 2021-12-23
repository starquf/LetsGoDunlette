using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_C_Sign : SkillPiece
{
    public GameObject contractAnimObj;

    public override void Cast()
    {
        BattleHandler bh = GameManager.Instance.battleHandler;

        AnimObj a = Instantiate(contractAnimObj, bh.player.transform.position, Quaternion.identity).GetComponent<AnimObj>();

        a.Play("Contract");

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
