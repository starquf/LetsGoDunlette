using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class FA_Skill : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override PieceInfo ChoiceSkill()
    {
        base.ChoiceSkill();
        if (Random.Range(0, 100) <= value)
        {
            onCastSkill = FA_Fairy_Ligtht;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = FA_Kidding;
            return pieceInfo[1];
        }
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void FA_Fairy_Ligtht(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "강화").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.2f);

            //전투가 끝날 때 까지 기본 공격의 피해가 5 상승한다.
            for (int i = 0; i < owner.skills.Count; i++)
            {
                FA_Attack skill = owner.skills[i].GetComponent<FA_Attack>();
                if (skill != null)
                {
                    skill.AddValue(5);
                }
            }

            animHandler.GetAnim(AnimName.M_Shield).SetPosition(owner.transform.position)
            .SetScale(1)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    private void FA_Kidding(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "공격").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.2f);

            target.GetDamage(Value, this, owner);

            animHandler.GetAnim(AnimName.M_Scratch).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
           {
               SetIndicator(owner.gameObject, "조각변경").OnEnd(() =>
               {
                   KiddingSkill();
                   onCastEnd?.Invoke();
               });
           });
        });
    }

    private void KiddingSkill() //현재 룰렛에 존재하는 플레이어의 룰렛 조각 중 하나를 적의 기본 공격으로 변경한다.
    {
        // 1. 안쓴 조각에서 attack이 있는가?
        // 2. 룰렛 안에 플레이어의 스킬이 있는가?

        // 3. 서로 바꿔야됩니다. <-

        List<SkillPiece> unUsedinven = GameManager.Instance.inventoryHandler.unusedSkills;

        if (!TryFindAttackFromAndCall(unUsedinven, FindRandomPlayerSkillAndChangePiece))
        {
            List<SkillPiece> usedinven = GameManager.Instance.inventoryHandler.usedSkills;
            TryFindAttackFromAndCall(usedinven, FindRandomPlayerSkillAndChangePiece);
        }
    }

    private bool TryFindAttackFromAndCall(List<SkillPiece> list, Action<SkillPiece> action)
    {
        for (int i = 0; i < list.Count; i++)
        {
            FA_Attack attack = list[i].GetComponent<FA_Attack>();

            if (attack != null)
            {
                if (attack.owner == owner)
                {
                    action?.Invoke(attack);
                    return true;
                }
            }
        }

        return false;
    }

    private void FindRandomPlayerSkillAndChangePiece(SkillPiece rulletPiece)
    {
        List<RulletPiece> list = GameManager.Instance.battleHandler.mainRullet.GetPieces();

        for (int j = 0; j < list.Count; j++)
        {
            if (list[j] == null)
            {
                continue;
            }

            SkillPiece skill = list[j].GetComponent<SkillPiece>();

            if (skill.isPlayerSkill)
            {
                GameManager.Instance.inventoryHandler.GetSkillFromInventory(rulletPiece);
                GameManager.Instance.battleHandler.mainRullet.ChangePiece(j, rulletPiece);

                return;
            }
        }
    }


}
