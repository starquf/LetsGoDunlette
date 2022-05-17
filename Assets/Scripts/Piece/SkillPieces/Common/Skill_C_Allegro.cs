using System;

public class Skill_C_Allegro : SkillPiece
{

    protected override void Start()
    {
        base.Start();

        isTargeting = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        animHandler.GetTextAnim()
        .SetType(TextUpAnimType.Up)
        .SetPosition(bh.mainRullet.transform.position)
        .Play("∑Í∑ø º”µµ ¡ı∞°!");

        Owner.GetComponent<LivingEntity>().AddShield(value);
        bh.mainRullet.RulletSpeed += 100f;

        onCastEnd?.Invoke();
    }
}
