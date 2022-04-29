using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RF_Skill : SkillPiece
{
    private GameObject presentgSkill; // 할퀴기

    private BattleHandler bh;
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

    private void RF_Sharp_Claw(LivingEntity target, Action onCastEnd = null) //상처를 부여해서 3턴 동안 10의 피해를 입힌다.
    {
            SetIndicator(owner.gameObject, "공격").OnEnd(() =>
            {
                target.GetDamage(10, this, owner);

                Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
                effect.transform.position = GameManager.Instance.enemyEffectTrm.position; effect.SetScale(2);
                effect.Play(() =>
                {
                    SetIndicator(owner.gameObject, "상처 부여").OnEnd(() =>
                    {
                        target.cc.SetCC(CCType.Wound, 3,true);
                        Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
                        effect.transform.position = bh.playerImgTrans.position;
                        effect.Play(() =>
                        {
                            onCastEnd?.Invoke();
                        });
                    });
                });
            });
    }

    private void RF_Sneaky(LivingEntity target, Action onCastEnd = null) //인벤토리에 '여우의 선물'을 2개 추가한다.
    {
        SetIndicator(owner.gameObject, "공격").OnEnd(() =>
        {
            target.GetDamage(20, this, owner);

            animHandler.GetAnim(AnimName.M_Butt)
            .SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2f)
            .Play(() =>
            {
                SetIndicator(owner.gameObject, "조각 추가").OnEnd(() =>
                {
                    for (int i = 0; i < 2; i++)
                    {
                        bh.battleUtil.SetTimer(0.25f * i, () => { ih.CreateSkill(presentgSkill, owner, owner.transform.position); });
                    }

                     bh.battleUtil.SetTimer(0.5f + 0.25f * 1, onCastEnd);
                });
            });
        });
    }


}
