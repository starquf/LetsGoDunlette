using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_Bamboo_Forest : SkillPiece
{
    private GameObject bambooSpear;

    protected override void Start()
    {
        base.Start();
        bambooSpear = GameManager.Instance.skillContainer.GetSkillPrefab<Skill_N_Bamboo_Spear>();
    }
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        return desInfos;
    }


    public override void Cast(LivingEntity target, Action onCastEnd = null) //자연 개수 + 개
    {
        animHandler.GetAnim(AnimName.SkillEffect01)
        .SetPosition(Owner.transform.position)
        .SetScale(2f)
        .SetRotation(Vector3.forward * -90f)
        .Play(() =>
        {
            List<SkillPiece> rulletPieces = GameManager.Instance.inventoryHandler.GetPlayerInventory().skills;
            int count = 0;
            for (int i = 0; i < rulletPieces.Count; i++)
            {
                if (rulletPieces[i] != null)
                {
                    if (rulletPieces[i].patternType == ElementalType.Nature)
                    {
                        count++;
                        bh.battleUtil.SetTimer(0.10f * i, () => { GameManager.Instance.inventoryHandler.CreateSkill(bambooSpear, Owner, Owner.transform.position); });
                    }
                }
            }
            bh.battleUtil.SetTimer(0.10f, () => { GameManager.Instance.inventoryHandler.CreateSkill(bambooSpear, Owner, Owner.transform.position); });
            bh.battleUtil.SetTimer(0.10f * count, onCastEnd);
        });
    }
}
