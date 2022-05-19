using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RF_Skill : SkillPiece
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
        if (Random.Range(0, 100) <= value)
        {
            desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(pieceInfo[0].GetValue())}");
            desInfos[1].SetInfo(DesIconType.Wound, $"{pieceInfo[0].GetValue(1)}턴");

            onCastSkill = RF_Sharp_Claw;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = RF_Sneaky;
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

    private void RF_Sharp_Claw(LivingEntity target, Action onCastEnd = null) //상처를 부여해서 3턴 동안 10의 피해를 입힌다.
    {
        SetIndicator(Owner.gameObject, "상처 부여").OnEndAction(() =>
        {
            target.GetDamage(GetDamageCalc(pieceInfo[0].GetValue()), this, Owner);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                target.cc.SetCC(CCType.Wound, pieceInfo[0].GetValue(1), true);
                onCastEnd?.Invoke();
            });
        });
    }

    private void RF_Sneaky(LivingEntity target, Action onCastEnd = null) //인벤토리에 '여우의 선물'을 3개 추가한다.
    {
        SetIndicator(Owner.gameObject, "조각 추가").OnEndAction(() =>
        {
            animHandler.GetAnim(AnimName.SkillEffect01)
            .SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2f)
            .Play(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    bh.battleUtil.SetTimer(0.25f * i, () => { ih.CreateSkill(presentgSkill, Owner, Owner.transform.position); });
                }

                bh.battleUtil.SetTimer(0.5f + (0.25f * 1), onCastEnd);
            });
        });
    }
}
