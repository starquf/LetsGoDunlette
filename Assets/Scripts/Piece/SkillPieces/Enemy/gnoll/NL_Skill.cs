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

    public override PieceInfo ChoiceSkill()
    {
        base.ChoiceSkill();
        if (Random.Range(0, 100) <= value)
        {
            onCastSkill = NL_Poison_Dagger;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = NL_Mark;
            return pieceInfo[1];
        }
    }


    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void NL_Poison_Dagger(LivingEntity target, Action onCastEnd = null) //��ó�� �ο��ؼ� 2�� ���� 10�� ���ظ� ������..
    {
        SetIndicator(owner.gameObject, "����").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
            effect.transform.position = GameManager.Instance.enemyEffectTrm.position; effect.SetScale(2);

            target.GetDamage(20 + addAdditionalDamage, this, owner);

            effect.Play(() =>
            {
                SetIndicator(owner.gameObject, "��ó�ο�").OnEnd(() =>
                {
                    target.cc.SetCC(CCType.Wound, 2, true);
                    onCastEnd?.Invoke();
                });
            });
        });
    }

    private void NL_Mark(LivingEntity target, Action onCastEnd = null) //���� ��� ������ ���ذ� 5 ����Ѵ�. ��ó ����
    {
        SetIndicator(owner.gameObject, "��ȭ").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
            effect.transform.position = GameManager.Instance.enemyEffectTrm.position; effect.SetScale(2);

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
        });
    }


}
