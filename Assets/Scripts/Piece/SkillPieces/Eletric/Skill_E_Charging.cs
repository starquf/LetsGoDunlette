using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Skill_E_Charging : SkillPiece
{
    private Action<SkillPiece, Action> onCharge = null;

    public Text counterText;
    private int attackCount = 0;
    private SkillEvent eventInfo = null;

    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);

    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;

        isTargeting = false;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(Value)}+{attackCount * Value}");

        return desInfos;
    }

    public override void OnRullet()
    {
        GameManager.Instance.battleHandler.battleEvent.RemoveEventInfo(eventInfo);
        onCharge = (piece, action) =>
        {
            if (piece.currentType.Equals(ElementalType.Electric) && piece != this)
            {
                animHandler.GetTextAnim()
               .SetType(TextUpAnimType.Up)
               .SetPosition(skillIconImg.transform.position)
               .SetScale(0.8f)
               .Play("ÃæÀüµÊ!");

                LogCon log = new LogCon
                {
                    text = $"ÃæÀüµÊ",
                    selfSpr = skillIconImg.sprite
                };

                DebugLogHandler.AddLog(LogType.ImageText, log);

                animHandler.GetAnim(AnimName.E_Static_Stun)
                .SetPosition(skillIconImg.transform.position)
                .SetScale(0.8f)
                .Play();

                HighlightColor(0.2f);

                attackCount++;
                counterText.text = $"+{attackCount * Value}";
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

        attackCount = 0;
        counterText.text = $"+{attackCount * Value}";
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        bh.battleUtil.StartCoroutine(ChargeAttack(target, onCastEnd));
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
        

        int damage = GetDamageCalc(Value)+(attackCount* Value);

        LivingEntity enemy = targets[Random.Range(0, targets.Count)];

        enemy.GetDamage(damage, currentType);

        animHandler.GetAnim(AnimName.E_Static)
        .SetScale(0.8f)
        .SetPosition(enemy.transform.position)
        .Play();

        GameManager.Instance.cameraHandler.ShakeCamera(1.5f, 0.15f);

        attackCount = 0;

        onCastEnd?.Invoke();
    }
}
