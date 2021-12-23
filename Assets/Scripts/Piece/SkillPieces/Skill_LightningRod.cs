using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_LightningRod : SkillPiece
{
    public GameObject LightningRodEffectPrefab;

    public override void Cast()
    {
        base.Cast();
        print($"스킬 발동!! 이름 : {PieceName}");

        Vector3 target = GameManager.Instance.battleHandler.enemy.transform.position;
        target.y -= 0.7f;
        target.x += 0.5f;

        Effect_LightningRod lightningRodEffect = Instantiate(LightningRodEffectPrefab, target, Quaternion.identity).GetComponent<Effect_LightningRod>();

        lightningRodEffect.Play("LightningRodEffect", () => {

        });

        GameManager.Instance.battleHandler.enemy.GetDamage(Value);
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

    }
}
