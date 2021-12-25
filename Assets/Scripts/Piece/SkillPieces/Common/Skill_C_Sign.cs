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
         
        // 클로저 생성
        Action<RulletPiece> onNextTest = result => { };

        onNextTest = result =>
        {
            if (result.PieceType.Equals(PieceType.SKILL))
            {
                print("안녕");
                bh.enemy.GetDamage(10);
                GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
            }
            
            // 바로 없엘거면 이렇게
            bh.onNextAttack -= onNextTest;
        };

        // 이벤트에 추가해주면 됨
        bh.onNextAttack += onNextTest;

        */

        bh.SetContract(Value, 3);
    }
}
