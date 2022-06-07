using System;
using System.Collections.Generic;

public class QN_Skill : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override PieceInfo ChoiceSkill() //알고리즘 정리. 종속자가 없을때는 55,45 확률로 백귀야행과 여왕의권위를 고름 아니라면 45,55의 확률로 여왕의권위와 보호를 고름
    {
        base.ChoiceSkill();

        if (UnityEngine.Random.Range(0, 100) <= 60)
        {
            desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(pieceInfo[0].GetValue())}");
            onCastSkill = QN_Authority;
            return pieceInfo[0];
        }
        else
        {
            desInfos[0].SetInfo(DesIconType.Shield, $"{pieceInfo[1].GetValue()}");
            onCastSkill = QN_Protection;
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

    private void QN_Authority(LivingEntity target, Action onCastEnd = null) //종속자가 있다면 추가 피해를 (<sprite=4>4) 준다.
    {
        SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.2f);
            int damage = GetDamageCalc(pieceInfo[0].GetValue());

            List<EnemyHealth> enemys = bh.enemys;

            for (int i = 0; i < enemys.Count; i++)
            {
                EnemyHealth health = enemys[i];

                if (health.gameObject != Owner.gameObject) //이거에 걸리면 종속자가 1명이상 있는것.
                {
                    damage += 4;
                    break;
                }
            }

            target.GetDamage(damage, this, Owner);

            animHandler.GetAnim(AnimName.M_Butt).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    private void QN_Protection(LivingEntity target, Action onCastEnd = null) //여왕'과 '종속자'에게 (<sprite=6>10)을  준다.
    {
        GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

        List<EnemyHealth> enemys = bh.enemys;

        for (int i = 0; i < enemys.Count; i++)
        {
            EnemyHealth health = enemys[i];

            if (health.gameObject != Owner.gameObject)
            {
                health.AddShield(pieceInfo[1].GetValue());
                animHandler.GetAnim(AnimName.M_Shield).SetPosition(health.transform.position)
                .SetScale(1)
                .Play();
            }
        }

        Owner.GetComponent<LivingEntity>().AddShield(pieceInfo[1].GetValue());

        animHandler.GetAnim(AnimName.M_Shield).SetPosition(Owner.transform.position)
            .SetScale(1)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
    }
}
