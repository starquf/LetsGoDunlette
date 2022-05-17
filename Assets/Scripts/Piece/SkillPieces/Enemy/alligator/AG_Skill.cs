using System;
using Random = UnityEngine.Random;

public class AG_Skill : SkillPiece
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
            onCastSkill = AG_Diving;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = AG_Crocodile_Bird;
            return pieceInfo[1];
        }
    }


    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void AG_Diving(LivingEntity target, Action onCastEnd = null) //2턴간 침묵을 받고 아무 데미지도 받지 않는다. 2턴이 지나면 플레이어에게 60만큼 피해를 입힌다.
    {
        Action<Action> onStartBattle = action =>
        {
            print("AG");
            SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
            {
                target.GetDamage(60);

                animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
                  .SetScale(2)
                  .Play(() =>
                  {
                      action?.Invoke();
                  });
            });
        };

        NormalEvent eventInfo = new NormalEvent(true, 3, onStartBattle, EventTime.EndOfTurn);
        bh.battleEvent.BookEvent(eventInfo);

        SetIndicator(Owner.gameObject, "침묵").OnEndAction(() =>
        {
            Owner.GetComponent<EnemyHealth>().cc.SetCC(CCType.Silence, 3);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
           {
               SetIndicator(Owner.gameObject, "무적").OnEndAction(() =>
               {
                   Owner.GetComponent<EnemyHealth>().cc.SetCC(CCType.Invincibility, 3);

                   animHandler.GetAnim(AnimName.M_Butt)
                           .SetPosition(GameManager.Instance.enemyEffectTrm.position)
                           .SetScale(2f)
                           .Play(() =>
                           {
                               onCastEnd?.Invoke();
                           });
               });
           });
        });
    }

    private void AG_Crocodile_Bird(LivingEntity target, Action onCastEnd = null) //자신의 체력을 40만큼 회복한다.
    {
        SetIndicator(Owner.gameObject, "회복").OnEndAction(() =>
        {
            Owner.GetComponent<EnemyHealth>().Heal(30);

            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            animHandler.GetAnim(AnimName.M_Recover).SetPosition(Owner.transform.position)
            .SetScale(1)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }


}
