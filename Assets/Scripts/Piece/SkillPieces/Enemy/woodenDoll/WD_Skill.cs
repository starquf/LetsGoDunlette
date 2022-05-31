using System;
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
            .SetPosition(Owner.transform.position)
            .SetScale(2f)
            .SetRotation(Vector3.forward * -90f)
            .Play(() =>
            {
                ih.CreateSkill(GetRandomSpell(), Owner, Owner.transform.position);

                bh.battleUtil.SetTimer(0.5f, onCastEnd);
            });
        });
    }

    private GameObject GetRandomSpell()
    {
        int rand = 0;

        rand = Random.Range(0, 100);

        return rand < 35 ? skill_FallingRock : rand < 65 ? skill_Refraction : rand < 95 ? skill_Water_Pressure : skill_Error;
    }
}
