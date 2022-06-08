using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TA_Skill : SkillPiece
{
    public Sprite heatingSprite;
    public Sprite normalSprite;

    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override PieceInfo ChoiceSkill()
    {
        base.ChoiceSkill();
        if (Random.Range(0, 100) < 70)  // 신체가열
        {
            onCastSkill = TA_Body_Heating;
            desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(pieceInfo[0].GetValue())}");

            usedIcons.Add(DesIconType.Attack);

            return pieceInfo[0];
        }
        else
        {
            onCastSkill = TA_Patrol;
            return pieceInfo[1];
        }
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        return desInfos;
    }

    public override void OnRullet()
    {
        base.OnRullet();
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void TA_Body_Heating(LivingEntity target, Action onCastEnd = null) //신체가열
    {
        SetIndicator(Owner.gameObject, "신체 가열").OnEndAction(() =>
        {
            target.GetDamage(GetDamageCalc(pieceInfo[0].GetValue()), this, Owner);

            animHandler.GetAnim(AnimName.M_Sword)
            .SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    private void TA_Patrol(LivingEntity target, Action onCastEnd = null) //순찰
    {
        SetIndicator(Owner.gameObject, "순찰").OnEndAction(() =>
        {
            Owner.GetComponent<Taros>().patrolCount += 3;

            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            animHandler.GetAnim(AnimName.M_Recover).SetPosition(Owner.transform.position)
            .SetScale(1)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    /*            Owner.GetComponent<SpriteRenderer>().sprite = heatingSprite;
            Owner.GetComponent<EnemyHealth>().cc.IncreaseBuff(BuffType.Upgrade, 3);

            bh.battleEvent.BookEvent(new NormalEvent(true, 3, action =>
            {
                Owner.GetComponent<EnemyHealth>().cc.DecreaseBuff(BuffType.Upgrade, 3);
                action?.Invoke();
            }, EventTime.EndOfTurn));*/
}
