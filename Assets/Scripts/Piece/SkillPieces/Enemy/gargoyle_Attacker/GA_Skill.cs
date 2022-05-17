using System;
using Random = UnityEngine.Random;

public class GA_Skill : SkillPiece
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
            onCastSkill = GA_Solidification;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = GA_Press;
            return pieceInfo[1];
        }
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void GA_Solidification(LivingEntity target, Action onCastEnd = null) // ������ ����� ������ ���� 5�� ��ȣ���� ��´�. ��ø����
    {
        SetIndicator(Owner.gameObject, "��ȣ�� �߰�").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            Owner.GetComponent<GA_Solidification>().shieldValue += 5;
            animHandler.GetAnim(AnimName.M_Shield).SetPosition(Owner.transform.position)
            .SetScale(1)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    private void GA_Press(LivingEntity target, Action onCastEnd = null) //���� �ڽ��� ��ȣ����ŭ �÷��̾�� �߰� ���ظ� �ش�.
    {
        SetIndicator(Owner.gameObject, "����").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            target.GetDamage(5, this, Owner);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
           {
               int curShield = Owner.GetComponent<EnemyHealth>().GetShieldHp();
               if (curShield > 0)
               {
                   SetIndicator(Owner.gameObject, $"�߰� ���� +{curShield}").OnEndAction(() =>
                   {
                       target.GetDamage(curShield, this, Owner);

                       animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
                       .SetScale(2)
                       .Play(() =>
                       {
                           onCastEnd?.Invoke();
                       });
                   });
               }
               else
               {
                   onCastEnd?.Invoke();
               }
           });
        });

    }


}
