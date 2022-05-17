using System;

public class SR_Skill : SkillPiece
{
    private Action<SkillPiece, Action> skillEvent;
    private SkillEvent skillEventInfo = null;

    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void OnRullet()
    {
        base.OnRullet();
        bh.battleEvent.RemoveEventInfo(skillEventInfo);

        skillEvent = (piece, action) =>
        {
            pieceDes = StringFormatUtil.GetEnemyAttackString(Value);

            action?.Invoke();
        };

        skillEventInfo = new SkillEvent(EventTimeSkill.WithSkill, skillEvent);
        bh.battleEvent.BookEvent(skillEventInfo);
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        SR_CutOff(target, onCastEnd);
    }

    private void SR_CutOff(LivingEntity target, Action onCastEnd = null) // 플레이어에게 30의 피해를 입힌다. //가르가 죽으면 절단의 데미지가 30만큼 증가한다.
    {
        SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            target.GetDamage(Value, this, Owner);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    public void SR_Sacrifice(Action onCastEnd = null) // 플레이어에게 30의 피해를 입힌다. //가르가 죽으면 절단의 데미지가 30만큼 증가한다.
    {
        GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

        AddValue(30);

        if (animHandler == null)
        {
            animHandler = GameManager.Instance.animHandler;
        }

        animHandler.GetAnim(AnimName.M_Recover).SetPosition(Owner.transform.position)
            .SetScale(1)
            .Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }





}
