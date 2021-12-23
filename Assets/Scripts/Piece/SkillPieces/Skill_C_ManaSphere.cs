using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_C_ManaSphere : SkillPiece
{
    public GameObject animObj;

    public override void Cast()
    {
        Vector3 target = GameManager.Instance.battleHandler.player.transform.position;

        AnimObj a = Instantiate(animObj, target, Quaternion.identity).GetComponent<AnimObj>();
        a.Play("normalHitEffect01", () => {
            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
        });

        GameManager.Instance.battleHandler.enemy.GetDamage(Value);
    }
}
