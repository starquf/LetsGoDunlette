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
            ResetDesInfo();
            desInfos[1].SetInfo(DesIconType.Upgrade, $"{pieceInfo[0].GetValue()}��"); //��ȭ ���������� �����ؾ���

            onCastSkill = FA_Fairy_Ligtht;
            return pieceInfo[0];
        }
        else
        {
            ResetDesInfo();
            desInfos[1].SetInfo(DesIconType.Wound, $"{pieceInfo[0].GetValue()}��");

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
        SetIndicator(Owner.gameObject, "��ȭ").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.2f);

            //������ ���� �� ���� �⺻ ������ ���ذ� 5 ����Ѵ�.
            for (int i = 0; i < Owner.skills.Count; i++)
            {
                FA_Attack skill = Owner.skills[i].GetComponent<FA_Attack>();
                if (skill != null)
                {
                    skill.AddValue(5);
                }
            }

            animHandler.GetAnim(AnimName.M_Shield).SetPosition(Owner.transform.position)
            .SetScale(1)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    private void FA_Kidding(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "����").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.2f);

            target.GetDamage(Value, this, Owner);

            animHandler.GetAnim(AnimName.M_Scratch).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
           {
               SetIndicator(Owner.gameObject, "��������").OnEndAction(() =>
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
            FA_Attack attack = list[i].GetComponent<FA_Attack>();

            if (attack != null)
            {
                if (attack.Owner == Owner)
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
                GameManager.Instance.inventoryHandler.GetSkillFromInventory(rulletPiece);
                mainRullet.ChangePiece(j, rulletPiece);

                return;
            }
        }
    }


}
