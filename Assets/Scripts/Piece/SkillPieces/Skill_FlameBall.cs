using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_FlameBall : SkillPiece
{
    public GameObject animObj;

    public override void Cast()
    {
        Vector3 target = GameManager.Instance.battleHandler.player.transform.position;

        AnimObj a = Instantiate(animObj, target, Quaternion.identity).GetComponent<AnimObj>();
        a.Play("fireEffect01", () => {
            print("이벤트 끝남");
        });

        GameManager.Instance.battleHandler.enemy.GetDamage(Value);
    }
}
