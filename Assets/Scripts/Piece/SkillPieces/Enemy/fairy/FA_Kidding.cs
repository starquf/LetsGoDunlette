using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FA_Kidding : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        print($"�� ��ų �ߵ�!! �̸� : {PieceName}");
        GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

        Vector3 targetPos = target.transform.position;
        Vector3 startPos = skillImg.transform.position;

        for (int i = 0; i < 3; i++)
        {
            EffectObj attackObj = PoolManager.GetItem<EffectObj>();
            attackObj.transform.position = startPos;

            int a = i;

            attackObj.Play(targetPos, () =>
            {
                attackObj.Sr.DOFade(0f, 0.1f)
                    .OnComplete(() =>
                    {
                        attackObj.EndEffect();
                    });

                GameManager.Instance.cameraHandler.ShakeCamera(0.25f, 0.2f);

                if (a == 2)
                {
                    onCastEnd?.Invoke();
                }
            }
            , BezierType.Cubic, i * 0.1f);

        }

        target.GetDamage(Value, this, owner);
        KiddingSkill();
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

        owner.GetComponent<EnemyIndicator>().ShowText("���� ����");
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
            if (list[j] == null) continue;

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
