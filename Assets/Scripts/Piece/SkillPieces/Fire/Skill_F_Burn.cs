using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_F_Burn : SkillPiece
{
    public Sprite effectSpr;
    private Gradient effectGradient;


    protected override void Start()
    {
        base.Start();
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.Fire];
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Wound, value.ToString());

        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        Vector3 targetPos = target.transform.position;

        animHandler.GetAnim(AnimName.Anim_FireEffect01)
                .SetPosition(targetPos)
                .Play(() =>
                {
                    target.cc.SetCC(CCType.Wound, value);

                    if (target.cc.IsCC(CCType.Wound))
                    {
                        animHandler.GetAnim(AnimName.Anim_FireEffect02)
                        .SetPosition(targetPos)
                        .Play(() =>
                        {
                            onCastEnd?.Invoke();
                        });
                        target.GetDamage(5);
                        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
                    }
                    else 
                    {
                        onCastEnd?.Invoke();
                    }
                });
    }
}
