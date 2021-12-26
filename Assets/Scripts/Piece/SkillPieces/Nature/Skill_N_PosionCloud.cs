using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_PosionCloud : SkillPiece
{
    public GameObject posionCloudEffectPrefab;

    public override void Cast(Action onCastEnd = null)
    {
        base.Cast();
        print($"스킬 발동!! 이름 : {PieceName}");

        Vector3 target = GameManager.Instance.battleHandler.enemy.transform.position;

        Anim_PosionCloud posionCloudEffect = PoolManager.GetItem<Anim_PosionCloud>();
        posionCloudEffect.transform.position = target;

        posionCloudEffect.Play(() => {
            GameManager.Instance.battleHandler.enemy.GetComponent<SpriteRenderer>().color = Color.green;
            GameManager.Instance.battleHandler.enemy.cc.SetCC(CCType.Wound, 1);
            onCastEnd?.Invoke();
        });

        GameManager.Instance.battleHandler.enemy.GetDamage(Value);
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
    }
}
