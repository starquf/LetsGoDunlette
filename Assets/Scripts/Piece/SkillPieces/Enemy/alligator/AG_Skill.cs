using System;
using System.Collections.Generic;
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
            desInfos[0].SetInfo(DesIconType.Shield, $"{pieceInfo[0].GetValue()}");
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = AG_Crocodile_Bird;
            desInfos[0].SetInfo(DesIconType.Heal, $"{pieceInfo[1].GetValue()}");
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

    private void AG_Diving(LivingEntity target, Action onCastEnd = null) //��ȣ���� 2�� �ȿ� ������� �ʴ´ٸ� ���ظ� 10 �ְ� ��ȣ���� ���� �ı��Ѵ�.
    {
        Action<Action> onStartBattle = action =>
        {
            LivingEntity livingEntity = Owner.GetComponent<LivingEntity>();
            if (livingEntity.GetShieldHp() > 0)
            {
                SetIndicator(Owner.gameObject, "����").OnEndAction(() =>
                {
                    target.GetDamage(10, this, Owner);
                    livingEntity.RemoveShield();

                    animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
                      .SetScale(2)
                      .Play(() =>
                      {
                          action?.Invoke();
                      });
                });
            }
            else
            {
                action?.Invoke();
            }
        };

        NormalEvent eventInfo = new NormalEvent(true, 3, onStartBattle, EventTime.EndOfTurn);
        bh.battleEvent.BookEvent(eventInfo);

        SetIndicator(Owner.gameObject, "ħ��").OnEndAction(() =>
        {
            EnemyHealth enemyHealth = Owner.GetComponent<EnemyHealth>();
            enemyHealth.cc.SetCC(CCType.Silence, 3);
            enemyHealth.AddShield(pieceInfo[0].GetValue());
        });
    }

    private void AG_Crocodile_Bird(LivingEntity target, Action onCastEnd = null) //�ڽ��� ü���� 40��ŭ ȸ���Ѵ�.
    {
        SetIndicator(Owner.gameObject, "ȸ��").OnEndAction(() =>
        {
            Owner.GetComponent<EnemyHealth>().Heal(pieceInfo[1].GetValue());

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
