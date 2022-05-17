using System;

public class TA_Attack : SkillPiece
{
    private int firstValue;
    private Action<SkillPiece, Action> onNextTurn;
    private SkillEvent eventInfo = null;

    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
        firstValue = Value;

        pieceDes = StringFormatUtil.GetEnemyAttackString(Value);
    }

    public override void OnRullet()
    {
        base.OnRullet();
        BattleHandler bh = GameManager.Instance.battleHandler;

        bh.battleEvent.RemoveEventInfo(eventInfo);

        onNextTurn = (piece, action) =>
        {
            if (Owner.GetComponent<EnemyHealth>().cc.IsCC(CCType.Heating))
            {
                SetValue(firstValue + 10);
            }
            else
            {
                SetValue(firstValue);
            }

            pieceDes = StringFormatUtil.GetEnemyAttackString(Value);

            action?.Invoke();
        };

        eventInfo = new SkillEvent(EventTimeSkill.AfterSkill, onNextTurn);
        bh.battleEvent.BookEvent(eventInfo);
    }

    public override void ResetPiece()
    {
        base.ResetPiece();

        GameManager.Instance.battleHandler.battleEvent.RemoveEventInfo(eventInfo);
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "АјАн").OnEndAction(() =>
        {
            if (Owner.GetComponent<EnemyHealth>().cc.IsCC(CCType.Heating))
            {
                target.GetDamage(Value, this, Owner);
            }
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }
}
