using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_N_Pigeon : SkillPiece
{
    public Sprite pigeonSpr;
    private Gradient effectGradient;

    protected override void Start()
    {
        base.Start();
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.Nature];

        bh = GameManager.Instance.battleHandler;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{Mathf.Clamp(GetDamageCalc() / 5, 1, int.MaxValue)}x5");
        desInfos[1].SetInfo(DesIconType.Silence, "2");

        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) //적에게 60피해를 입힌다.		침묵	-	적에게 2턴간 침묵을 부여한다.
    {
        Vector3 targetPos = target.transform.position;
        Vector3 startPos = Owner.transform.position;

        int damage = Mathf.Clamp(GetDamageCalc() / 5, 1, int.MaxValue);

        for (int i = 0; i < 5; i++)
        {
            int a = i;

            startPos.x = Random.Range(-1.8f, 1.8f);

            EffectObj skillEffect = PoolManager.GetItem<EffectObj>();
            skillEffect.transform.position = startPos;
            skillEffect.SetSprite(pigeonSpr);
            skillEffect.SetColorGradient(effectGradient);
            skillEffect.SetScale(Vector3.one * 0.7f);

            skillEffect.Play(targetPos, () =>
            {
                animHandler.GetAnim(AnimName.N_ManaSphereHit)
                .SetPosition(targetPos)
                .SetScale(0.5f)
                .Play();

                target.GetDamage(damage, currentType);

                GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

                if (a >= 4)
                {
                    target.cc.SetCC(CCType.Silence, 3);
                    onCastEnd?.Invoke();
                }

                skillEffect.EndEffect();
            }, BezierType.Cubic, isRotate: true, playSpeed: 2f, delay: i * 0.05f);
        }
    }
}
