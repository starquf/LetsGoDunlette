using System;
using UnityEngine;

public class TA_Skill : SkillPiece
{
    public Sprite heatingSprite;
    public Sprite normalSprite;

    public int skillCount = 0;
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override PieceInfo ChoiceSkill()
    {
        base.ChoiceSkill();
        if (!Owner.GetComponent<EnemyHealth>().cc.IsCC(CCType.Heating))
        {
            if (skillCount < 2)
            {
                onCastSkill = TA_Off_Limits;

                Owner.GetComponent<SpriteRenderer>().sprite = normalSprite;

                for (int i = 0; i < Owner.skills.Count; i++)
                {
                    TA_Skill skill = Owner.skills[i].GetComponent<TA_Skill>();
                    if (skill != null)
                    {
                        skill.pieceInfo[0].PieceDes = "플레이어에게 40의 피해를 입힌다.";
                    }
                }

                return pieceInfo[0];
            }
            else
            {
                onCastSkill = TA_Body_Heating;
                return pieceInfo[1];
            }

        }
        else
        {
            onCastSkill = TA_Off_Limits;
            return pieceInfo[0];
        }
    }


    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void TA_Off_Limits(LivingEntity target, Action onCastEnd = null) //플레이어에게 40의 피해를 입힌다.
    {
        SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
        {
            if (Owner.GetComponent<EnemyHealth>().cc.IsCC(CCType.Heating))
            {
                target.GetDamage(50, this, Owner);
            }
            else
            {
                target.GetDamage(40, this, Owner);
            }

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });

            for (int i = 0; i < Owner.skills.Count; i++)
            {
                TA_Skill skill = Owner.skills[i].GetComponent<TA_Skill>();
                if (skill != null)
                {
                    skill.skillCount++;
                }
            }
        });
    }

    private void TA_Body_Heating(LivingEntity target, Action onCastEnd = null) //타로스의 모든 공격의 피해를 10만큼 증가시킨다. //스프라이트 변경
    {
        SetIndicator(Owner.gameObject, "강화").OnEndAction(() =>
        {
            Owner.GetComponent<SpriteRenderer>().sprite = heatingSprite;
            Owner.GetComponent<EnemyHealth>().cc.SetCC(CCType.Heating, 4);

            for (int i = 0; i < Owner.skills.Count; i++)
            {
                TA_Skill skill = Owner.skills[i].GetComponent<TA_Skill>();
                if (skill != null)
                {
                    skill.pieceInfo[0].PieceDes = "플레이어에게 50의 피해를 입힌다.";
                    skill.skillCount = 0;
                }
            }

            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            animHandler.GetAnim(AnimName.M_Recover).SetPosition(Owner.transform.position)
            .SetScale(1)
            .Play(() =>
           {
               onCastEnd?.Invoke();
           });
        });
    }


}
