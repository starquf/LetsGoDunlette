using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DP_Skill : SkillPiece
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
            onCastSkill = DP_Duty;
            pieceInfo[0].PieceDes = string.Format(pieceInfo[0].PieceDes, pieceInfo[0].GetValue(), pieceInfo[0].GetValue(1));
            return pieceInfo[0];
        }
        else
        {
            desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(pieceInfo[1].GetValue())}"); //��ȭ ���������� �����ؾ���

            onCastSkill = DP_Poke;
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

    private void DP_Duty(LivingEntity target, Action onCastEnd = null)
    {
        EnemyHealth boss = null;

        List<EnemyHealth> enemys = bh.enemys;

        for (int i = 0; i < enemys.Count; i++)
        {
            EnemyHealth health = enemys[i];

            if (health.isBoss)
            {
                boss = health;
                // queen ��.
                break; // ������ 1���̶�� ����
            }
        }

        if (boss == null)
        {
            Debug.LogError("������ ����");
            return;
        }
        SetIndicator(Owner.gameObject, "����").OnEndAction(() =>
        {
            Owner.GetComponent<LivingEntity>().GetDamageIgnoreShild(pieceInfo[0].GetValue());

            animHandler.GetAnim(AnimName.M_Wisp)
            .SetPosition(Owner.transform.position)
            .SetScale(1);

            SetIndicator(boss.gameObject, "ȸ��").OnEndAction(() =>
            {
                GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
                boss.Heal(pieceInfo[0].GetValue(1));

                animHandler.GetAnim(AnimName.M_Recover).SetPosition(boss.transform.position)
                .SetScale(1)
                .Play(() =>
                    {
                        onCastEnd?.Invoke();
                    });
                });
        });
    }

    private void DP_Poke(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "����").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });

            target.GetDamage(GetDamageCalc(pieceInfo[1].GetValue()), this, Owner);
        });
    }


}
