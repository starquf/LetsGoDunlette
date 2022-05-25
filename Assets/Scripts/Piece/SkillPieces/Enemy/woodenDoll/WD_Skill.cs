using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WD_Skill : SkillPiece
{
    private GameObject skill_FallingRock;
    private GameObject skill_Refraction;
    private GameObject skill_Water_Pressure;
    private GameObject skill_Error;

    private InventoryHandler ih;

    protected override void Awake()
    {
        base.Awake();

        isPlayerSkill = false;
    }

    protected override void Start()
    {
        base.Start();

        skill_FallingRock = GameManager.Instance.skillContainer.GetSkillPrefab<WD_S_Falling_Rocks>();
        skill_Refraction = GameManager.Instance.skillContainer.GetSkillPrefab<WD_S_Refraction>();
        skill_Water_Pressure = GameManager.Instance.skillContainer.GetSkillPrefab<WD_S_Water_Pressure>();
        skill_Error = GameManager.Instance.skillContainer.GetSkillPrefab<WD_S_Error>();

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
                    bh.battleUtil.SetTimer(0.25f * i, () => { ih.CreateSkill(GetRandomSpell(), Owner, Owner.transform.position); });
                }

                bh.battleUtil.SetTimer(0.5f + (0.25f * 1), onCastEnd);
            });
        });
    }

    private GameObject GetRandomSpell()
    {
        int rand = 0;

        rand = Random.Range(0, 100);

        if (rand < 35)      // 낙석
        {
            return skill_FallingRock;
        }
        else if (rand < 65) // 굴절
        {
            return skill_Refraction;
        }
        else if (rand < 95) // 수압
        {
            return skill_Water_Pressure;
        }
        else // 오류
        {
            return skill_Error;
        }
    }
}
