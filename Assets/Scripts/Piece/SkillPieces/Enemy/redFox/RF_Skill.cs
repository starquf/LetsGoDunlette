using System;
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
            onCastSkill = RF_Sharp_Claw;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = RF_Sneaky;
            return pieceInfo[1];
        }
    }


    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void RF_Sharp_Claw(LivingEntity target, Action onCastEnd = null) //��ó�� �ο��ؼ� 3�� ���� 10�� ���ظ� ������.
    {
        SetIndicator(Owner.gameObject, "����").OnEndAction(() =>
        {
            target.GetDamage(10, this, Owner);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                SetIndicator(Owner.gameObject, "��ó �ο�").OnEndAction(() =>
                {
                    target.cc.SetCC(CCType.Wound, 3, true);
                    animHandler.GetAnim(AnimName.M_Sword).SetPosition(bh.playerImgTrans.position)
                    .SetScale(1)
                    .Play(() =>
                    {
                        onCastEnd?.Invoke();
                    });
                });
            });
        });
    }

    private void RF_Sneaky(LivingEntity target, Action onCastEnd = null) //�κ��丮�� '������ ����'�� 2�� �߰��Ѵ�.
    {
        SetIndicator(Owner.gameObject, "����").OnEndAction(() =>
        {
            target.GetDamage(20, this, Owner);

            animHandler.GetAnim(AnimName.M_Butt)
            .SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2f)
            .Play(() =>
            {
                SetIndicator(Owner.gameObject, "���� �߰�").OnEndAction(() =>
                {
                    for (int i = 0; i < 2; i++)
                    {
                        bh.battleUtil.SetTimer(0.25f * i, () => { ih.CreateSkill(presentgSkill, Owner, Owner.transform.position); });
                    }

                    bh.battleUtil.SetTimer(0.5f + (0.25f * 1), onCastEnd);
                });
            });
        });
    }


}
