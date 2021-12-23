using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_PosionCloud : SkillPiece
{
    public GameObject posionCloudEffectPrefab;

    public override void Cast()
    {
        base.Cast();
        print($"스킬 발동!! 이름 : {PieceName}");

        Vector3 target = GameManager.Instance.battleHandler.enemy.transform.position;

        Effect_PosionCloud posionCloudEffect = Instantiate(posionCloudEffectPrefab, target, Quaternion.identity).GetComponent<Effect_PosionCloud>();

        posionCloudEffect.Play("PosionCloudEffect", () => {
            GameManager.Instance.battleHandler.enemy.GetComponent<SpriteRenderer>().color = Color.green;
        });

        GameManager.Instance.battleHandler.enemy.GetDamage(Value);
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

    }
}
