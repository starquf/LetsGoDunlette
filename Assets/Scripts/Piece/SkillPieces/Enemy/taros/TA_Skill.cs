using System;
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
        if (Random.Range(0, 100) < 70)  // ���ٱ���
        {
            onCastSkill = TA_Off_Limits;

            desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(Value)}");

            Owner.GetComponent<SpriteRenderer>().sprite = normalSprite;

            return pieceInfo[0];
        }
        else
        {
            onCastSkill = TA_Body_Heating;
            return pieceInfo[1];
        }
    }


    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void TA_Off_Limits(LivingEntity target, Action onCastEnd = null) //�÷��̾�� 40�� ���ظ� ������.
    {
        SetIndicator(Owner.gameObject, "����").OnEndAction(() =>
        {
            target.GetDamage(GetDamageCalc(), this, Owner);

            animHandler.GetAnim(AnimName.M_Sword)
            .SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    private void TA_Body_Heating(LivingEntity target, Action onCastEnd = null) //Ÿ�ν��� ��� ������ ���ظ� 10��ŭ ������Ų��. //��������Ʈ ����
    {
        SetIndicator(Owner.gameObject, "��ȭ").OnEndAction(() =>
        {
            Owner.GetComponent<SpriteRenderer>().sprite = heatingSprite;
            Owner.GetComponent<EnemyHealth>().cc.SetBuff(BuffType.Upgrade, 3);

            bh.battleEvent.BookEvent(new NormalEvent(true, 3, action => 
            {
                Owner.GetComponent<EnemyHealth>().cc.DecreaseBuff(BuffType.Upgrade, 3);
                action?.Invoke();
            }, EventTime.EndOfTurn));

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
