using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_ManaSphere : SkillPiece
{
    public Sprite manaSphereSpr;
    private Gradient effectGradient;

    protected override void Start()
    {
        base.Start();
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.Nature];
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());

        return desInfos;
    }

    private int GetDamageCalc()
    {
        int attack = (int)(Owner.GetComponent<LivingEntity>().AttackPower * 0.4f);

        return attack;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        //print($"스킬 발동!! 이름 : {PieceName}");

        Vector3 targetPos = target.transform.position;
        Vector3 startPos = transform.position;

        EffectObj skillEffect = PoolManager.GetItem<EffectObj>();
        skillEffect.transform.position = startPos;
        skillEffect.SetSprite(manaSphereSpr);
        skillEffect.SetColorGradient(effectGradient);
        skillEffect.SetScale(Vector3.one);

        skillEffect.Play(targetPos, () =>
        {
            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
            target.GetDamage(GetDamageCalc(), currentType);

            onCastEnd?.Invoke();

            animHandler.GetAnim(AnimName.N_ManaSphereHit)
                .SetPosition(targetPos)
                .Play();

            skillEffect.EndEffect();
        }, BezierType.Linear, isRotate: true, playSpeed: 2f);
    }
}
