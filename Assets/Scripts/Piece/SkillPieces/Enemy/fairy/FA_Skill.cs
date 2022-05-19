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
        SetIndicator(Owner.gameObject, "��ų ��ȭ").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.2f);

            //������ ���� �� ���� �⺻ ������ ���ذ� pieceInfo[0].GetValue() ����Ѵ�.
            for (int i = 0; i < Owner.skills.Count; i++)
            {
                FA_Attack skill = Owner.skills[i].GetComponent<FA_Attack>();
                if (skill != null)
                {
                    AddAttackPower(GetDamageCalc(pieceInfo[0].GetValue()));
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

            target.GetDamage(GetDamageCalc(pieceInfo[1].GetValue()), this, Owner);

            animHandler.GetAnim(AnimName.M_Scratch).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
           {
               SetIndicator(Owner.gameObject, "�峭�ٷ���").OnEndAction(() =>
               {
                   KiddingSkill(); //KIding �� ��ŵ
                   onCastEnd?.Invoke();
               });
           });
        });
    }

    private void KiddingSkill() //���� �귿�� �����ϴ� �÷��̾��� �귿 ���� �� �ϳ��� ���� �⺻ �������� �����Ѵ�. ���� : �κ��丮�� �� ��ų ������ 1�� �߰��Ѵ�.
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
