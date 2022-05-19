using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RF_Skill : SkillPiece
{
    private GameObject presentgSkill; // ������

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
            desInfos[1].SetInfo(DesIconType.Wound, $"{pieceInfo[0].GetValue(1)}��");

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

    private void RF_Sharp_Claw(LivingEntity target, Action onCastEnd = null) //��ó�� �ο��ؼ� 3�� ���� 10�� ���ظ� ������.
    {
        SetIndicator(Owner.gameObject, "��ó �ο�").OnEndAction(() =>
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

    private void RF_Sneaky(LivingEntity target, Action onCastEnd = null) //�κ��丮�� '������ ����'�� 3�� �߰��Ѵ�.
    {
        SetIndicator(Owner.gameObject, "���� �߰�").OnEndAction(() =>
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
