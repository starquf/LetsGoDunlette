using System;
using System.Collections.Generic;

public class Skill_F_Berserker : SkillPiece
{
    protected override void Start()
    {
        base.Start();
        bh = GameManager.Instance.battleHandler;
    }
    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        //���� ü�� 10�� �߰� ���ظ�  (<sprite=3>4)�ش�.
        var living = Owner.GetComponent<LivingEntity>();
        int addDamage = (living.maxHp - living.curHp) / 10 * 4;
        if (addDamage > 0)
        {
            //Ÿ���� ����
            target.GetDamage(addDamage,currentType);
        }
        onCastEnd?.Invoke();
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        return desInfos;
    }
}
