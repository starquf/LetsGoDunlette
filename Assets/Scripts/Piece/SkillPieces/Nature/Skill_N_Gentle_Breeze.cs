using System;

public class Skill_N_Gentle_Breeze : SkillPiece
{

    public override void Cast(LivingEntity target, Action onCastEnd = null) //체력을 20 회복한 후 자연필드를 생성한다.
    {
        Owner.GetComponent<PlayerHealth>().Heal(value);
        bh.fieldHandler.SetFieldType(ElementalType.Nature);

        animHandler.GetAnim(AnimName.M_Recover).SetPosition(Owner.transform.position)
            .SetScale(1)
            .Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }
}
