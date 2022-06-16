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

        animHandler.GetAnim(AnimName.F_Arson)
                .SetPosition(targetPos)
                .SetRotation(Vector3.forward * 90f)
                .Play(() =>
                {
                    if(target.cc.IsCC(CCType.Wound))
                    {
                        target.GetDamage(5);
                    }
                    target.cc.SetCC(CCType.Wound, value);
                    GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

                    onCastEnd?.Invoke();
                });
    }
}