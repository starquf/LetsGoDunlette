using System;

public class SA_Skill : SkillPiece
{
    public bool sacrificed = false;
    private Action<SkillPiece, Action> skillEvent;
    private SkillEvent skillEventInfo = null;

    private Action<EnemyHealth, Action> enemyEvent;
    private EnemyEvent enemyEventInfo = null;
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

        skillEventInfo = new SkillEvent(EventTimeSkill.AfterSkill, skillEvent);
        bh.battleEvent.BookEvent(skillEventInfo);
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        SR_CutOff(target, onCastEnd);
    }

    private void SR_CutOff(LivingEntity target, Action onCastEnd = null) // 플레이어에게 30의 피해를 입힌다. //가르가 죽으면 절단의 데미지가 30만큼 증가한다.
    {
        SetIndicator(owner.gameObject, "공격").OnEnd(() =>
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

    private void Sacrifice()
    {
        BattleHandler battleHandler = GameManager.Instance.battleHandler;

        battleHandler.battleEvent.RemoveEventInfo(enemyEventInfo);

        enemyEvent = (enemy, action) =>
        {
            SkillPiece result = this;

            result.HighlightColor(0.4f);

            Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
            textEffect.SetType(TextUpAnimType.Damage);
            textEffect.transform.position = result.skillImg.transform.position;
            textEffect.Play($"{result.PieceName} 발동!");

            Anim_M_Wisp wispEffect = PoolManager.GetItem<Anim_M_Wisp>();
            wispEffect.transform.position = result.skillImg.transform.position;
            wispEffect.SetScale(1.5f);

            wispEffect.Play(() =>
            {
                if (result != null)
                {
                    StartCoroutine(battleHandler.battleEvent.ActionEvent(EventTimeSkill.WithSkill, result));

                    Inventory temp = result.owner;

                    result.owner = this.owner;
                    result.Cast(battleHandler.player, () =>
                    {
                        result.owner = temp;
                        action.Invoke();
                    });
                }
                else
                {
                    action?.Invoke();
                }
            });
        };

        enemyEventInfo = new EnemyEvent(EventTimeEnemy.EnemyDie, enemyEvent);
        battleHandler.battleEvent.BookEvent(enemyEventInfo);
    }

}
