using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WD_Skill : SkillPiece
{
    private GameObject presentgSkill; // 할퀴기

    private InventoryHandler ih;

    protected override void Awake()
    {
        base.Awake();

        isPlayerSkill = false;
    }

    protected override void Start()
    {
        base.Start();

        presentgSkill = GameManager.Instance.skillContainer.GetSkillPrefab<RF_Present>();

        bh = GameManager.Instance.battleHandler;
        ih = GameManager.Instance.inventoryHandler;
    }

    public override PieceInfo ChoiceSkill()
    {
        base.ChoiceSkill();

        onCastSkill = WD_Mana_Charging;

        return pieceInfo[0];
    }
    public override List<DesIconInfo> GetDesIconInfo()
    {
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void WD_Mana_Charging(LivingEntity target, Action onCastEnd = null) // 조각을 2개 추가한다.
    {
        SetIndicator(Owner.gameObject, "조각 추가").OnEndAction(() =>
        {
            animHandler.GetAnim(AnimName.SkillEffect01)
            .SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2f)
            .Play(() =>
            {
                for (int i = 0; i < 2; i++)
                {
                    bh.battleUtil.SetTimer(0.25f * i, () => { ih.CreateSkill(presentgSkill, Owner, Owner.transform.position); });
                }

                bh.battleUtil.SetTimer(0.5f + (0.25f * 1), onCastEnd);
            });
        });
    }
}
