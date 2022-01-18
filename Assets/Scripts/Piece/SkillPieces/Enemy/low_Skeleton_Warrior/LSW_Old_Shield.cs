using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSW_Old_Shield : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        print($"적 스킬 발동!! 이름 : {PieceName}");
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

        Vector3 targetPos = target.transform.position;
        Vector3 startPos = skillImg.transform.position;

        for (int i = 0; i < 3; i++)
        {
            EffectObj attackObj = PoolManager.GetItem<EffectObj>();
            attackObj.transform.position = startPos;

            int a = i;

            attackObj.Play(targetPos, () =>
            {
                attackObj.Sr.DOFade(0f, 0.1f)
                    .OnComplete(() =>
                    {
                        attackObj.EndEffect();
                    });

                GameManager.Instance.cameraHandler.ShakeCamera(0.25f, 0.2f);

                if (a == 2)
                    onCastEnd?.Invoke();
            }
            , BezierType.Cubic, i * 0.1f);
        }

        target.AddShield(value);
    }
}
