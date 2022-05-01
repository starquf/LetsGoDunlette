using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Skill_E_Charging : SkillPiece
{
    private BattleHandler bh = null;
    private Action<SkillPiece,Action> onCharge = null;

    public Text counterText;
    private int attackCount = 1;
        SkillEvent eventInfo = null;

    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);

    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;

        hasTarget = false;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc().ToString()}x{attackCount}");

        return desInfos;
    }

    private int GetDamageCalc()
    {
        int attack = (int)(owner.GetComponent<LivingEntity>().AttackPower * 0.2f + 1);

        return attack;
    }

    public override void OnRullet()
    {
        GameManager.Instance.battleHandler.battleEvent.RemoveEventInfo(eventInfo);
        onCharge = (piece,action) =>
        {
            if (piece.currentType.Equals(ElementalType.Electric) && piece != this)
            {
                animHandler.GetTextAnim()
               .SetType(TextUpAnimType.Up)
               .SetPosition(skillImg.transform.position)
               .SetScale(0.8f)
               .Play("충전됨!");

                LogCon log = new LogCon();
                log.text = $"충전됨";
                log.selfSpr = skillImg.sprite;

                DebugLogHandler.AddLog(LogType.ImageText, log);

                animHandler.GetAnim(AnimName.E_Static_Stun)
                .SetPosition(skillImg.transform.position)
                .SetScale(0.8f)
                .Play();

                HighlightColor(0.2f);

                attackCount++;
                counterText.text = attackCount.ToString();
            }
            action?.Invoke();
        };

        eventInfo = new SkillEvent(EventTimeSkill.WithSkill, onCharge);
        GameManager.Instance.battleHandler.battleEvent.BookEvent(eventInfo);
    }

    public override void ResetPiece()
    {
        base.ResetPiece();

        GameManager.Instance.battleHandler.battleEvent.RemoveEventInfo(eventInfo);

        attackCount = 1;
        counterText.text = attackCount.ToString();
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        GameManager.Instance.battleHandler.battleUtil.StartCoroutine(ChargeAttack(target, onCastEnd));
    }

    private IEnumerator ChargeAttack(LivingEntity target, Action onCastEnd = null)
    {
        if (bh.enemys.Count <= 0)
        {
            onCastEnd?.Invoke();
            yield break;
        }

        List<LivingEntity> targets = new List<LivingEntity>();

        if (target == bh.player)
        {
            targets.Add(target);
        }
        else
        {
            targets = bh.battleUtil.DeepCopyEnemyList(bh.enemys);
        }

        int atkCnt = attackCount;

        int damage = GetDamageCalc();

        for (int i = 0; i < atkCnt; i++)
        {
            LivingEntity enemy = targets[Random.Range(0, targets.Count)];

            enemy.GetDamage(GetDamageCalc(), currentType);

            LogCon log = new LogCon();
            log.text = $"{GetDamageCalc()} 데미지 부여";
            log.selfSpr = skillImg.sprite;
            log.targetSpr = enemy.GetComponent<SpriteRenderer>().sprite;

            DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);

            animHandler.GetAnim(AnimName.E_LightningRod)
            .SetScale(0.8f)
            .SetPosition(enemy.transform.position)
            .Play();

            GameManager.Instance.cameraHandler.ShakeCamera(1.5f, 0.15f);

            yield return pOneSecWait;
        }

        attackCount = 1;

        onCastEnd?.Invoke();
    }
}
