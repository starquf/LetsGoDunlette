using System;

public class Skill_N_Gentle_Breeze : SkillPiece
{

    public override void Cast(LivingEntity target, Action onCastEnd = null) //ü���� 20 ȸ���� �� �ڿ��ʵ带 �����Ѵ�.
    {
        Owner.GetComponent<PlayerHealth>().Heal(value);
        GameManager.Instance.battleHandler.fieldHandler.SetFieldType(ElementalType.Nature);

        animHandler.GetAnim(AnimName.M_Recover).SetPosition(Owner.transform.position)
            .SetScale(1)
            .Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }
}
