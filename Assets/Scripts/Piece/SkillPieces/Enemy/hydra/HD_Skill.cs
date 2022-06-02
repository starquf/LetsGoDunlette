using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HD_Skill : SkillPiece
{
    private GameObject skill_poison;

    private InventoryHandler ih;

    protected override void Awake()
    {
        base.Awake();

        isPlayerSkill = false;
    }

    protected override void Start()
    {
        base.Start();

        skill_poison = GameManager.Instance.skillContainer.GetSkillPrefab<HD_Poison>();

        bh = GameManager.Instance.battleHandler;
        ih = GameManager.Instance.inventoryHandler;
    }

    public override PieceInfo ChoiceSkill()
    {
        base.ChoiceSkill();

        if (Random.Range(0, 100) < 50)
        {
            onCastSkill = HD_Tail_Swing;
            desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(pieceInfo[0].GetValue())}");
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = HD_Poison_Spray;
            return pieceInfo[1];
        }
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void HD_Tail_Swing(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
        {
            target.GetDamage(GetDamageCalc(pieceInfo[0].GetValue()), this, Owner);

            animHandler.GetAnim(AnimName.M_Butt).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    private void HD_Poison_Spray(LivingEntity target, Action onCastEnd = null) // 조각을 3개 추가한다.
    {
        SetIndicator(Owner.gameObject, "조각 추가").OnEndAction(() =>
        {
            animHandler.GetAnim(AnimName.SkillEffect01)
            .SetPosition(Owner.transform.position)
            .SetScale(2f)
            .SetRotation(Vector3.forward * -90f)
            .Play(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    bh.battleUtil.SetTimer(0.25f * i, () => { ih.CreateSkill(skill_poison, Owner, Owner.transform.position); });
                }

                bh.battleUtil.SetTimer(0.5f + (0.25f * 2), onCastEnd);
            });
        });
    }
}
