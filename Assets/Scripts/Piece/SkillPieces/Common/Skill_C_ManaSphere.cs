using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_C_ManaSphere : SkillPiece
{
    public Sprite manaSphereSpr;

    private Gradient effectGradient;

    protected override void Start()
    {
        base.Start();
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.None];
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        Vector3 startPos = Owner.transform.position;
        Vector3 targetPos = target.transform.position;

        animHandler.GetAnim(AnimName.C_SphereCast)
            .SetPosition(targetPos)
            .Play(() =>
            {
                EffectObj effect = PoolManager.GetItem<EffectObj>();
                effect.transform.position = startPos;
                effect.SetSprite(manaSphereSpr);
                effect.SetColorGradient(effectGradient);
                effect.SetScale(Vector3.one);

                effect.Play(targetPos, () =>
                {
                    GameManager.Instance.cameraHandler.ShakeCamera(1f, 0.2f);
                    target.GetDamage(GetDamageCalc(), currentType);
                    onCastEnd?.Invoke();

                    animHandler.GetAnim(AnimName.C_ManaSphereHit)
                        .SetPosition(targetPos)
                        .Play();

                    effect.EndEffect();
                }, BezierType.Linear, isRotate: true, playSpeed: 2f);
            });
    }
}
