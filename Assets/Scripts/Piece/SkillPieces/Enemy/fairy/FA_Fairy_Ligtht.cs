using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FA_Fairy_Ligtht : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        print($"적 스킬 발동!! 이름 : {PieceName}");
        GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.2f);

        Vector3 targetPos = target.transform.position;
        Vector3 startPos = skillImg.transform.position;

        //전투가 끝날 때 까지 기본 공격의 피해가 5 상승한다.
        for (int i = 0; i < owner.skills.Count; i++)
        {
            var skill = owner.skills[i].GetComponent<FA_Attack>();
            if (skill != null)
            {
                skill.AddValue(value);
            }
        }

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

        owner.GetComponent<EnemyIndicator>().ShowText("강화");
    }
}
