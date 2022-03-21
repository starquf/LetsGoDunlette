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

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        if (Random.Range(0, 100) <= value)
        {
            FA_Fairy_Ligtht(target, onCastEnd);
        }
        else
        {
            FA_Kidding(target, onCastEnd);
        }
    }

    private void FA_Fairy_Ligtht(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "��ȭ").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.2f);

            //������ ���� �� ���� �⺻ ������ ���ذ� 5 ����Ѵ�.
            for (int i = 0; i < owner.skills.Count; i++)
            {
                FA_Attack skill = owner.skills[i].GetComponent<FA_Attack>();
                if (skill != null)
                {
                    skill.AddValue(5);
                }
            }

            Anim_M_Recover effect = PoolManager.GetItem<Anim_M_Recover>();
            effect.transform.position = owner.transform.position;

            effect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    private void FA_Kidding(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "����").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.2f);

            target.GetDamage(Value, owner.gameObject);

            Anim_M_Scratch effect = PoolManager.GetItem<Anim_M_Scratch>();
            effect.transform.position = owner.transform.position;

            effect.Play(() =>
            {
                SetIndicator(owner.gameObject, "��������").OnEnd(() =>
                {
                    KiddingSkill();
                    onCastEnd?.Invoke();
                });
            });
        });
    }

    private void KiddingSkill() //���� �귿�� �����ϴ� �÷��̾��� �귿 ���� �� �ϳ��� ���� �⺻ �������� �����Ѵ�.
    {
        // 1. �Ⱦ� �������� attack�� �ִ°�?
        // 2. �귿 �ȿ� �÷��̾��� ��ų�� �ִ°�?

        // 3. ���� �ٲ�ߵ˴ϴ�. <-

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
