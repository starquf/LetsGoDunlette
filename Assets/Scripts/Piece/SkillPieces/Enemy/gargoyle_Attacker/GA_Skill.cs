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

    private void GA_Solidification(LivingEntity target, Action onCastEnd = null) // 전투가 종료될 때까지 매턴 5의 보호막을 얻는다. 중첩가능
    {
        SetIndicator(Owner.gameObject, "보호막 추가").OnEndAction(() =>
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

    private void GA_Press(LivingEntity target, Action onCastEnd = null) //현재 자신의 보호막만큼 플레이어에게 추가 피해를 준다.
    {
        SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
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
                   SetIndicator(Owner.gameObject, $"추가 피해 +{curShield}").OnEndAction(() =>
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
