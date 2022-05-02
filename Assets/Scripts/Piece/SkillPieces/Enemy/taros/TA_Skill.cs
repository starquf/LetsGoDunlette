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
        if (!owner.GetComponent<EnemyHealth>().cc.IsCC(CCType.Heating))
        {
            if (skillCount < 2)
            {
                onCastSkill = TA_Off_Limits;

                owner.GetComponent<SpriteRenderer>().sprite = normalSprite;

                for (int i = 0; i < owner.skills.Count; i++)
                {
                    TA_Skill skill = owner.skills[i].GetComponent<TA_Skill>();
                    if (skill != null)
                    {
                        skill.pieceInfo[0].PieceDes = "�÷��̾�� 40�� ���ظ� ������.";
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

    private void TA_Off_Limits(LivingEntity target, Action onCastEnd = null) //�÷��̾�� 40�� ���ظ� ������.
    {
        SetIndicator(owner.gameObject, "����").OnEnd(() =>
        {
            if (owner.GetComponent<EnemyHealth>().cc.IsCC(CCType.Heating))
            {
                target.GetDamage(50, this, owner);
            }
            else
            {
                target.GetDamage(40, this, owner);
            }

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
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

    private void TA_Body_Heating(LivingEntity target, Action onCastEnd = null) //Ÿ�ν��� ��� ������ ���ظ� 10��ŭ ������Ų��. //��������Ʈ ����
    {
        SetIndicator(owner.gameObject, "��ȭ").OnEnd(() =>
        {
            owner.GetComponent<SpriteRenderer>().sprite = heatingSprite;
            owner.GetComponent<EnemyHealth>().cc.SetCC(CCType.Heating, 4);

            for (int i = 0; i < owner.skills.Count; i++)
            {
                TA_Skill skill = owner.skills[i].GetComponent<TA_Skill>();
                if (skill != null)
                {
                    skill.pieceInfo[0].PieceDes = "�÷��̾�� 50�� ���ظ� ������.";
                    skill.skillCount = 0;
                }
            }

            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            animHandler.GetAnim(AnimName.M_Recover).SetPosition(owner.transform.position)
            .SetScale(1)
            .Play(() =>
           {
               onCastEnd?.Invoke();
           });
        });
    }


}
