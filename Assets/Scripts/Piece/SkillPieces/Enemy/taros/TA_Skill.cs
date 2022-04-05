using System;
using UnityEngine;

public class TA_Skill : SkillPiece
{
    public Sprite heatingSprite;
    bool isHeating = false;
    public int skillCount = 0;
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override PieceInfo ChoiceSkill()
    {
        base.ChoiceSkill();
        if (!isHeating)
        {
            if (skillCount < 2)
            {
                onCastSkill = TA_Off_Limits;
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
        SetIndicator(owner.gameObject, "공격").OnEnd(() =>
        {
            if (isHeating)
            {
                target.GetDamage(50, this, owner);
            }
            else
            {
                target.GetDamage(40, this, owner);
            }

            Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
            effect.transform.position = GameManager.Instance.enemyEffectTrm.position; effect.SetScale(2);
            effect.Play(() =>
            {
                onCastEnd?.Invoke();
            });

            for (int i = 0; i < owner.skills.Count; i++)
            {
                TA_Skill skill = owner.skills[i].GetComponent<TA_Skill>();
                if (skill != null)
                {
                    skill.skillCount++;
                }
            }
        });
    }

    private void TA_Body_Heating(LivingEntity target, Action onCastEnd = null) //타로스의 모든 공격의 피해를 10만큼 증가시킨다. //스프라이트 변경
    {
        SetIndicator(owner.gameObject, "강화").OnEnd(() =>
        {
            for (int i = 0; i < owner.skills.Count; i++)
            {
                TA_Attack skill = owner.skills[i].GetComponent<TA_Attack>();
                if (skill != null)
                {
                    skill.AddValue(10);
                    // break; // 1개 라고 가정함
                }
            }

            for (int i = 0; i < owner.skills.Count; i++)
            {
                TA_Skill skill = owner.skills[i].GetComponent<TA_Skill>();
                if (skill != null)
                {
                    skill.isHeating = true;
                }
            }

            owner.GetComponent<SpriteRenderer>().sprite = heatingSprite;

            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            Anim_M_Recover effect = PoolManager.GetItem<Anim_M_Recover>();
            effect.transform.position = owner.transform.position;

            effect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }


}
