using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_C_Agitato : SkillPiece
{
    private BattleHandler bh;

    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;

        isTargeting = true;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        int damageStack = (int)bh.mainRullet.RulletSpeed / 50;

        target.GetDamage(damageStack * 5, currentType);

        LogCon log = new LogCon();
        log.text = $"{damageStack * 5} 데미지 부여";
        log.selfSpr = skillImg.sprite;
        log.targetSpr = target.GetComponent<SpriteRenderer>().sprite;

        DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);

        onCastEnd?.Invoke();
    }
}
