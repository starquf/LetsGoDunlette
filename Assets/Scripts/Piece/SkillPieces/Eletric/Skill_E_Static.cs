using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_E_Static : SkillPiece
{
    public GameObject staticEffectPrefab;
    public GameObject staticStunEffectPrefab;

    [Header("기절 확률")]
    public int stunPercent;

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        BattleHandler bh = GameManager.Instance.battleHandler;
        //print($"스킬 발동!! 이름 : {PieceName}");

        Vector3 targetPos = target.transform.position;
        targetPos.y -= 0.7f;

        Anim_E_Static staticEffect = PoolManager.GetItem<Anim_E_Static>();
        staticEffect.transform.position = targetPos;

        staticEffect.Play(() => {
            target.GetDamage(Value, currentType);
            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
            if(Random.Range(0,100) < stunPercent)
            {
                Anim_E_Static_Stun stunEffect = PoolManager.GetItem<Anim_E_Static_Stun>();
                stunEffect.transform.position = targetPos;

                stunEffect.Play(() => {
                    onCastEnd?.Invoke();
                });

                target.cc.SetCC(CCType.Stun, 1);
            }
            else
            {
                onCastEnd?.Invoke();
            }
        });
    }
}
