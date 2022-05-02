using System;

public class Skill_N_Gentle_Breeze : SkillPiece
{

    public override void Cast(LivingEntity target, Action onCastEnd = null) //ü���� 20 ȸ���� �� �ڿ��ʵ带 �����Ѵ�.
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
