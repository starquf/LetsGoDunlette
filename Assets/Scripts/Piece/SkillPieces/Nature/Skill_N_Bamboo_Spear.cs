using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_Bamboo_Spear : SkillPiece
{
    public Sprite bambooSpear;

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());
        return desInfos;
    }


    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        BattleHandler bh = GameManager.Instance.battleHandler;
        InventoryHandler ih = GameManager.Instance.inventoryHandler;

        EffectObj effectObj = PoolManager.GetItem<EffectObj>();
        effectObj.SetSprite(bambooSpear);
        effectObj.SetScale(Vector3.one * 2f);
        effectObj.SetColorGradient(ih.effectGradDic[currentType]);
        effectObj.transform.position = Owner.transform.position;

        effectObj.Play(target.transform.position, type: BezierType.Linear, onEndEffect: () =>
        {
            GameManager.Instance.cameraHandler.ShakeCamera(1.5f, 0.2f);

            animHandler.GetAnim(AnimName.N_ManaSphereHit)
            .SetPosition(target.transform.position)
            .SetScale(1f)
            .Play();

            target.GetDamage(GetDamageCalc(Value));

            onCastEnd?.Invoke();

            effectObj.EndEffect();
        }, playSpeed: 2f);

        animHandler.GetAnim(AnimName.SkillEffect01)
        .SetPosition(Owner.transform.position + (Vector3.up * 1f))
        .SetScale(1.5f)
        .SetRotation(Vector3.forward * 90f)
        .Play(() =>
        {
        });
    }
}
