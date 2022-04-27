using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_PosionCloud : SkillPiece
{
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[1].SetInfo(DesIconType.Wound, "2");

        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        //print($"스킬 발동!! 이름 : {PieceName}");

        BattleHandler bh = GameManager.Instance.battleHandler;
        Vector3 targetPos = target.transform.position;

        Anim_N_PosionCloud posionCloudEffect = PoolManager.GetItem<Anim_N_PosionCloud>();
        posionCloudEffect.transform.position = targetPos;

        posionCloudEffect.Play(() => {
            onCastEnd?.Invoke();
        });

        target.cc.SetCC(CCType.Wound, 2);
    }
}
