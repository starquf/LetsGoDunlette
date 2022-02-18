using DG.Tweening;
using System;
using UnityEngine;

public class HP_Gliding : SkillPiece
{
    public GameObject scratchingSkill; // 할퀴기
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        print($"적 스킬 발동!! 이름 : {PieceName}");
        GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

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
                {
                    onCastEnd?.Invoke();
                }
            }
            , BezierType.Cubic, i * 0.1f);

        }

        GlidingSkill();

        //이펙트 부분
        Anim_M_Sword hitEffect = PoolManager.GetItem<Anim_M_Sword>();
        hitEffect.transform.position = owner.transform.position;

        hitEffect.Play(() =>
        {
        });
    }

    //스킬 부분
    public void GlidingSkill() //덱에 할퀴기 2개 집어넣는다.
    {
        Inventory owner1 = owner.GetComponent<EnemyInventory>();

        for (int i = 0; i < value; i++)
        {
            SkillPiece skill = GameManager.Instance.inventoryHandler.CreateSkill(scratchingSkill, owner1);
            GameManager.Instance.inventoryHandler.unusedSkills.Add(skill);
        }
        owner.GetComponent<EnemyIndicator>().ShowText("할퀴기 추가");
    }
}
