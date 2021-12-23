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
