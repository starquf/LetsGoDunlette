using System;

public class Skill_N_NaturalHealing : SkillPiece
{

    public override void Cast(LivingEntity target, Action onCastEnd = null) //ü���� 40 ȸ���Ѵ�.
    {
        owner.GetComponent<LivingEntity>().Heal(value);

        animHandler.GetAnim(AnimName.M_Recover).SetPosition(owner.transform.position)
            .SetScale(1)
            .Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }
}
