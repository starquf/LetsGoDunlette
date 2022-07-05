using System;

public class Skill_W_Change : SkillPiece
{
    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;
        isTargeting = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        LivingEntity entity = Owner.GetComponent<LivingEntity>();

        entity.ChangeShieldToHealth();

        animHandler.GetAnim(AnimName.M_Shield).SetPosition(Owner.transform.position)
             .SetScale(0.5f)
             .Play();

        animHandler.GetAnim(AnimName.W_Splash01)
            .SetPosition(entity.hpBar.transform.position)
            .SetScale(0.5f)
            .Play(() => onCastEnd?.Invoke());
    }

}
