using System;

public class Skill_N_Gentle_Breeze : SkillPiece
{

    public override void Cast(LivingEntity target, Action onCastEnd = null) //체력을 20 회복한 후 자연필드를 생성한다.
    {
        owner.GetComponent<PlayerHealth>().Heal(value);
        GameManager.Instance.battleHandler.fieldHandler.SetFieldType(ElementalType.Nature);

        animHandler.GetAnim(AnimName.M_Recover).SetPosition(owner.transform.position)
            .SetScale(1)
            .Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }
}
