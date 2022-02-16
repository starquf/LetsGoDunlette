using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MI_Skill : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        if (Random.Range(1, 100) <= value)
        {
            MI_Biting(target, onCastEnd);
        }
        else
        {
            MI_Bump(target, onCastEnd);
        }
    }

    private void MI_Biting(LivingEntity target, Action onCastEnd = null)
    {
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

        target.GetDamage(20, owner.gameObject);
        target.cc.SetCC(CCType.Wound, 5);

        owner.GetComponent<EnemyIndicator>().ShowText("상처 부여");

        Anim_M_Bite hitEffect = PoolManager.GetItem<Anim_M_Bite>();
        hitEffect.transform.position = owner.transform.position;

        hitEffect.Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }

    private void MI_Bump(LivingEntity target, Action onCastEnd = null)
    {
        GameManager.Instance.cameraHandler.ShakeCamera(0.7f, 0.15f);

        target.GetDamage(35, owner.gameObject);

        Anim_M_Bite hitEffect = PoolManager.GetItem<Anim_M_Bite>();
        hitEffect.transform.position = owner.transform.position;

        hitEffect.Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }


}
