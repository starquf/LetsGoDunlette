using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_C_T_Pigeon : SkillPiece
{
    public Sprite pigeonSpr;
    private Gradient effectGradient;

    protected override void Start()
    {
        base.Start();
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.None];
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Silence, Value.ToString());
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) //Ä§¹¬	3ÅÏ
    {
        Vector3 targetPos = target.transform.position;
        Vector3 startPos = Owner.transform.position;

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

                GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

                if (a >= 4)
                {
                    target.cc.SetCC(CCType.Silence, value + 1);
                    onCastEnd?.Invoke();
                }

                skillEffect.EndEffect();
            }, BezierType.Cubic, isRotate: true, playSpeed: 2f, delay: i * 0.05f);
        }
    }
}
