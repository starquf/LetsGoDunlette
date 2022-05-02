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

        BattleHandler bh = GameManager.Instance.battleHandler;
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

    private void SR_CutOff(LivingEntity target, Action onCastEnd = null) // �÷��̾�� 30�� ���ظ� ������. //������ ������ ������ �������� 30��ŭ �����Ѵ�.
    {
        SetIndicator(owner.gameObject, "����").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            target.GetDamage(Value, this, owner);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    public void SR_Sacrifice(Action onCastEnd = null) // �÷��̾�� 30�� ���ظ� ������. //������ ������ ������ �������� 30��ŭ �����Ѵ�.
    {
        GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

        AddValue(30);
        animHandler.GetAnim(AnimName.M_Recover).SetPosition(owner.transform.position)
            .SetScale(1)
            .Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }





}
