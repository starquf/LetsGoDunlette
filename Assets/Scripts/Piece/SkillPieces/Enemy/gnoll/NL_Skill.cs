using System;
using Random = UnityEngine.Random;

public class NL_Skill : SkillPiece
{
    private int addAdditionalDamage = 0;
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        if (Random.Range(1, 100) <= value)
        {
            NL_Poison_Dagger(target, onCastEnd);
        }
        else
        {
            NL_Mark(target, onCastEnd);
        }
    }

    private void NL_Poison_Dagger(LivingEntity target, Action onCastEnd = null) //��ó�� �ο��ؼ� 2�� ���� 10�� ���ظ� ������..
    {
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

        Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
        effect.transform.position = owner.transform.position;

        target.cc.SetCC(CCType.Wound, 2);

        owner.GetComponent<EnemyIndicator>().ShowText("��ó �ο�");

        target.GetDamage(20 + addAdditionalDamage, owner.gameObject);


        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });



    }

    private void NL_Mark(LivingEntity target, Action onCastEnd = null) //���� ��� ������ ���ذ� 5 ����Ѵ�. ��ó ����
    {
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

        Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
        effect.transform.position = owner.transform.position;

        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });

        addAdditionalDamage += 5;

        for (int i = 0; i < owner.skills.Count; i++)
        {
            NL_Attack skill = owner.skills[i].GetComponent<NL_Attack>();
            if (skill != null)
            {
                skill.AddValue(5);

                // break; // 1�� ��� ������
            }
        }
        owner.GetComponent<EnemyIndicator>().ShowText("��ȭ");


    }


}
