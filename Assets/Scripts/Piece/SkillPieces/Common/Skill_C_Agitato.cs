using System;
using UnityEngine;

public class Skill_C_Agitato : SkillPiece
{
    protected override void Start()
    {
        base.Start();

        isTargeting = true;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        int damageStack = (int)bh.mainRullet.RulletSpeed / 50;

        target.GetDamage(damageStack * 5, currentType);

        LogCon log = new LogCon
        {
            text = $"{damageStack * 5} ������ �ο�",
            selfSpr = skillImg.sprite,
            targetSpr = target.GetComponent<SpriteRenderer>().sprite
        };

        DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);

        onCastEnd?.Invoke();
    }
}
