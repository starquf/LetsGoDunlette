using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FA_Skill : SkillPiece
{
    private GameObject addSkill; // 할퀴기
    private InventoryHandler ih;
    private readonly string msg = "장난꾸러기 발동!";

    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    protected override void Start()
    {
        base.Start();
        addSkill = GameManager.Instance.skillContainer.GetSkillPrefab<FA_Skill>();
        ih = GameManager.Instance.inventoryHandler;
    }

    public override PieceInfo ChoiceSkill()
    {
        base.ChoiceSkill();
        if (Random.Range(0, 100) <= value)
        {
            desInfos[0].SetInfo(DesIconType.Upgrade, $"{pieceInfo[0].GetValue()}");

            onCastSkill = FA_Fairy_Ligtht;
            return pieceInfo[0];
        }
        else
        {
            desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(pieceInfo[1].GetValue())}");

            onCastSkill = FA_Kidding;
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

    private void FA_Fairy_Ligtht(LivingEntity target, Action onCastEnd = null)
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
                    bh.battleUtil.SetTimer(0.25f * i, () => { ih.CreateSkill(addSkill, Owner, Owner.transform.position); });
                }

                bh.battleUtil.SetTimer(0.5f + (0.25f * 1), onCastEnd);
            });
        });
    }

    private void FA_Kidding(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.2f);

            target.GetDamage(GetDamageCalc(pieceInfo[1].GetValue()), this, Owner);

            animHandler.GetAnim(AnimName.M_Scratch).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
           {
               /*
               SetIndicator(Owner.gameObject, "조각 변경").OnEndAction(() =>
               {
                   //KiddingSkill(); //KIding 은 스킵
                   onCastEnd?.Invoke();
               });*/
               onCastEnd?.Invoke();
           });
        });
    }
    /*
    private void KiddingSkill() //현재 룰렛에 존재하는 플레이어의 룰렛 조각 중 하나를 적의 기본 공격으로 변경한다. 현재 : 인벤토리에 페어리 스킬 조각을 1개 추가한다.
    {
        // 1. 안쓴 조각에서 attack이 있는가?
        // 2. 룰렛 안에 플레이어의 스킬이 있는가?

        // 3. 서로 바꿔야됩니다. <-

        InventoryHandler ih = GameManager.Instance.inventoryHandler;

        if (!TryFindAttackFromAndCall(Owner.skills, FindRandomPlayerSkillAndChangePiece))
        {
            List<SkillPiece> usedinven = GameManager.Instance.inventoryHandler.graveyard;
            TryFindAttackFromAndCall(usedinven, FindRandomPlayerSkillAndChangePiece);
        }
    }

    private bool TryFindAttackFromAndCall(List<SkillPiece> list, Action<SkillPiece> action)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Owner != Owner)
            {
                continue;
            }

            FA_Skill attack = list[i].GetComponent<FA_Skill>();

            if (attack != null)
            {
                action?.Invoke(attack);
                return true;
            }
        }

        return false;
    }

    private void FindRandomPlayerSkillAndChangePiece(SkillPiece rulletPiece)
    {
        SkillRullet mainRullet = bh.mainRullet;
        List<RulletPiece> list = mainRullet.GetPieces();

        for (int j = 0; j < list.Count; j++)
        {
            if (list[j] == null)
            {
                continue;
            }

            SkillPiece skill = list[j].GetComponent<SkillPiece>();

            if (skill.isPlayerSkill)
            {
                animHandler.GetAnim(AnimName.BlueExplosion01)
                    .SetPosition(skill.skillIconImg.transform.position)
                    .Play();

                animHandler.GetTextAnim()
                    .SetType(TextUpAnimType.Up)
                    .SetPosition(skill.skillIconImg.transform.position)
                    .Play(msg);

                GameManager.Instance.inventoryHandler.GetSkillFromInventoryOrGraveyard(rulletPiece);
                mainRullet.ChangePiece(j, rulletPiece);

                return;
            }
        }
    }*/


}
