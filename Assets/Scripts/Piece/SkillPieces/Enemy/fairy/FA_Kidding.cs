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
        print($"적 스킬 발동!! 이름 : {PieceName}");
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

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

        target.GetDamage(Value);
        KiddingSkill();
    }

    private void KiddingSkill() //현재 룰렛에 존재하는 플레이어의 룰렛 조각 중 하나를 적의 기본 공격으로 변경한다.
    {
        List<SkillPiece> unUsedinven = GameManager.Instance.inventoryHandler.unusedSkills;
        if (!TryFindAttackFromAndCall(unUsedinven, FindRandomPlayerSkillAndChangePiece))
        {
            List<SkillPiece> usedInven = GameManager.Instance.inventoryHandler.usedSkills;
            TryFindAttackFromAndCall(usedInven, FindRandomPlayerSkillAndChangePiece);
        }
    }

    private bool TryFindAttackFromAndCall(List<SkillPiece> list, Action<RulletPiece> action)
    {
        List<SkillPiece> usedInven = GameManager.Instance.inventoryHandler.usedSkills;
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

    private void FindRandomPlayerSkillAndChangePiece(RulletPiece rulletPiece)
    {
        List<RulletPiece> list = GameManager.Instance.battleHandler.mainRullet.GetPieces();
        for (int j = 0; j < list.Count; j++)
        {
            SkillPiece skill = list[j].GetComponent<SkillPiece>();
            if (skill.isPlayerSkill)
            {
                GameManager.Instance.battleHandler.mainRullet.ChangePiece(j, rulletPiece);
                return;
            }
        }
    }
}
