using System;
using System.Collections.Generic;

public class QN_Skill : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override PieceInfo ChoiceSkill() //�˰��� ����. �����ڰ� �������� 55,45 Ȯ���� ��;���� �����Ǳ����� �� �ƴ϶�� 45,55�� Ȯ���� �����Ǳ����� ��ȣ�� ��
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

    private void QN_Authority(LivingEntity target, Action onCastEnd = null) //�����ڰ� �ִٸ� �߰� ���ظ� (<sprite=4>4) �ش�.
    {
        SetIndicator(Owner.gameObject, "����").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.2f);
            int damage = GetDamageCalc(pieceInfo[0].GetValue());

            List<EnemyHealth> enemys = bh.enemys;

            for (int i = 0; i < enemys.Count; i++)
            {
                EnemyHealth health = enemys[i];

                if (health.gameObject != Owner.gameObject) //�̰ſ� �ɸ��� �����ڰ� 1���̻� �ִ°�.
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

    private void QN_Protection(LivingEntity target, Action onCastEnd = null) //����'�� '������'���� (<sprite=6>10)��  �ش�.
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
