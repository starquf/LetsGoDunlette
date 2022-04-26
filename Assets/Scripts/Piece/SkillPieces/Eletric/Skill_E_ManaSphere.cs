using System;
using System.Collections;
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

    private int GetDamageCalc()
    {
        int attack = (int)(owner.GetComponent<LivingEntity>().AttackPower * 0.4f);

        return attack;
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

        skillEffect.Play(targetPos, () => {
            Anim_E_ManaSphereHit hitEffect = PoolManager.GetItem<Anim_E_ManaSphereHit>();
            hitEffect.transform.position = targetPos;

            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
            target.GetDamage(GetDamageCalc(), currentType);

            LogCon log = new LogCon();
            log.text = $"{GetDamageCalc()} 데미지 부여";
            log.selfSpr = skillImg.sprite;
            log.targetSpr = target.GetComponent<SpriteRenderer>().sprite;

            DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);

            onCastEnd?.Invoke();

            hitEffect.Play();

            skillEffect.EndEffect();
        }, BezierType.Linear, isRotate: true, playSpeed: 2f);
    }
}
