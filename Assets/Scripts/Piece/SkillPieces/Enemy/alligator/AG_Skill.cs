using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
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
            desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(pieceInfo[1].GetValue())}");
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

    private void AG_Diving(LivingEntity target, Action onCastEnd = null) //보호막이 2턴 안에 사라지지 않는다면 피해를 10 주고 보호막을 전부 파괴한다.
    {
        Action<Action> onStartBattle = action =>
        {
            LivingEntity livingEntity = Owner.GetComponent<LivingEntity>();
            if (livingEntity.GetShieldHp() > 0)
            {
                animHandler.GetAnim(AnimName.T_WaterSplash05).SetPosition(Owner.transform.position)
                .SetScale(2f)
                .Play(() =>
                {
                    SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
                    {
                        target.GetDamage(pieceInfo[0].GetValue(1), this, Owner);
                        livingEntity.RemoveShield();

                        animHandler.GetAnim(AnimName.M_Bite).SetPosition(GameManager.Instance.enemyEffectTrm.position)
                          .SetScale(2)
                          .Play(() =>
                          {
                              action?.Invoke();
                          });
                    });
                });
                Owner.GetComponent<SpriteRenderer>().DOColor(Color.white, 0.4f);
            }
            else
            {
                animHandler.GetAnim(AnimName.T_WaterSplash05).SetPosition(Owner.transform.position)
                .SetScale(2f)
                .Play();
                Owner.GetComponent<SpriteRenderer>().DOColor(Color.white, 0.4f);
                action?.Invoke();
            }
        };

        NormalEvent eventInfo = new NormalEvent(true, 3, onStartBattle, EventTime.EndOfTurn);
        bh.battleEvent.BookEvent(eventInfo);
        animHandler.GetAnim(AnimName.T_WaterSplash05).SetPosition(Owner.transform.position)
        .SetScale(2f)
        .Play();
        Owner.GetComponent<SpriteRenderer>().DOColor(Color.clear, 0.3f);
        SetIndicator(Owner.gameObject, "침묵").OnEndAction(() =>
        {
            EnemyHealth enemyHealth = Owner.GetComponent<EnemyHealth>();
            enemyHealth.cc.SetCC(CCType.Silence, 3);
            enemyHealth.AddShield(pieceInfo[0].GetValue());
            onCastEnd?.Invoke();
        });

    }

    private void AG_Crocodile_Bird(LivingEntity target, Action onCastEnd = null) //자신의 체력을 40만큼 회복한다.
    {
        SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
        {
            target.GetDamage(GetDamageCalc(pieceInfo[1].GetValue()));

            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            animHandler.GetAnim(AnimName.M_Scratch).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(1)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }


}
