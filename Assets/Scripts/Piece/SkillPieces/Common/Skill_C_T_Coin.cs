using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_C_T_Coin : SkillPiece
{
    public Sprite coinSpr;
    private Gradient effectGradient;

    protected override void Start()
    {
        base.Start();
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.None];
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) //"50 골드를 얻는다. 전투 종료 시 또는 사용 후 삭제된다."
    {
        Vector3 targetPos = target.transform.position;
        Vector3 startPos = Owner.transform.position;


        EffectObj skillEffect = PoolManager.GetItem<EffectObj>();
        skillEffect.transform.position = startPos;
        skillEffect.SetSprite(coinSpr);
        skillEffect.SetColorGradient(effectGradient);
        skillEffect.SetScale(Vector3.one * 0.3f);


        skillEffect.Play(targetPos, () =>
        {
            animHandler.GetAnim(AnimName.C_ManaSphereHit)
            .SetPosition(targetPos)
            .SetScale(0.7f)
            .Play();

            target.GetDamage(1, currentType);

            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

            for (int i = 0; i < 10; i++)
            {
                int a = i;

                startPos.x = 0;

                EffectObj skillEffect1 = PoolManager.GetItem<EffectObj>();
                skillEffect1.transform.position = targetPos;
                skillEffect1.SetSprite(coinSpr);
                skillEffect1.SetColorGradient(effectGradient);
                skillEffect1.SetScale(Vector3.one * 0.3f);

                skillEffect1.Play(startPos, () =>
                {
                    animHandler.GetAnim(AnimName.C_ManaSphereHit)
                    .SetPosition(startPos)
                    .SetScale(0.5f)
                    .Play();

                    GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

                    if (a >= 9)
                    {
                        GameManager.Instance.AddGold(value);
                        onCastEnd?.Invoke();
                    }

                    skillEffect1.EndEffect();
                }, BezierType.Cubic, isRotate: true, playSpeed: 2f, delay: i * 0.05f);
            }

            skillEffect.EndEffect();
        }, BezierType.Linear, isRotate: true, playSpeed: 2f);
    }
}
