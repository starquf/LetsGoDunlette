using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_E_ManaSphere : SkillPiece
{
    public Sprite effectSpr;
    private Gradient effectGradient;

    protected override void Start()
    {
        base.Start();
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.Electric];
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());

        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        //print($"스킬 발동!! 이름 : {PieceName}");

        Vector3 targetPos = target.transform.position;
        Vector3 startPos = transform.position;

        EffectObj skillEffect = PoolManager.GetItem<EffectObj>();
        skillEffect.transform.position = startPos;
        skillEffect.SetSprite(effectSpr);
        skillEffect.SetColorGradient(effectGradient);
        skillEffect.SetScale(Vector3.one);

        skillEffect.Play(targetPos, () =>
        {

            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
            target.GetDamage(GetDamageCalc(), currentType);

            LogCon log = new LogCon
            {
                text = $"{GetDamageCalc()} 데미지 부여",
                selfSpr = skillImg.sprite,
                targetSpr = target.GetComponent<SpriteRenderer>().sprite
            };

            DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);

            onCastEnd?.Invoke();

            animHandler.GetAnim(AnimName.E_ManaSphereHit)
            .SetPosition(targetPos)
            .Play();

            skillEffect.EndEffect();
        }, BezierType.Linear, isRotate: true, playSpeed: 2f);
    }
}
