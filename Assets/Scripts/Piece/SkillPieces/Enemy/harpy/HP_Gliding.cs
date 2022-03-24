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
        SetIndicator(owner.gameObject, "할퀴기 추가").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            Anim_M_Sword hitEffect = PoolManager.GetItem<Anim_M_Sword>();
            hitEffect.transform.position = GameManager.Instance.enemyEffectTrm.position; hitEffect.SetScale(2);

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });

            GlidingSkill();

        });

    }

    //스킬 부분
    public void GlidingSkill() //덱에 할퀴기 2개 집어넣는다.
    {
        Inventory owner1 = owner.GetComponent<EnemyInventory>();

        for (int i = 0; i < value; i++)
        {
            GameManager.Instance.inventoryHandler.CreateSkill(scratchingSkill, owner1);
        }
    }
}
