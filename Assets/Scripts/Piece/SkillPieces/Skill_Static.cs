using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Static : SkillPiece
{
    public GameObject staticEffectPrefab;

    public override void Cast()
    {
        base.Cast();
        print($"스킬 발동!! 이름 : {PieceName}");

        Vector3 target = GameManager.Instance.battleHandler.enemy.transform.position;
        target.y -= 0.7f;

        Effect_Static staticEffect = Instantiate(staticEffectPrefab, target, Quaternion.identity).GetComponent<Effect_Static>();

        staticEffect.Play("StaticEffect", () => {
        });

        GameManager.Instance.battleHandler.enemy.GetDamage(Value);
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

    }
}
