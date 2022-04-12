using System;

public class SA_Skill : SkillPiece
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

            Anim_M_Bite hitEffect = PoolManager.GetItem<Anim_M_Bite>();
            hitEffect.transform.position = GameManager.Instance.enemyEffectTrm.position; hitEffect.SetScale(2);

            target.GetDamage(Value, this, owner);

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    public void SR_Sacrifice(Action onCastEnd = null) // �÷��̾�� 30�� ���ظ� ������. //������ ������ ������ �������� 30��ŭ �����Ѵ�.
    {
        GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

        Anim_M_Recover effect = PoolManager.GetItem<Anim_M_Recover>();
        effect.transform.position = GameManager.Instance.enemyEffectTrm.position; effect.SetScale(2);

        AddValue(30);

        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }





}
