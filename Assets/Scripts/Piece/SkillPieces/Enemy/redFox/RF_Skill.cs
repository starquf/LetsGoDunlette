using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RF_Skill : SkillPiece
{
    public GameObject presentgSkill; // ������
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        if (Random.Range(1, 100) <= value)
        {
            RF_Sharp_Claw(target, onCastEnd);
        }
        else
        {
            RF_Sneaky(target, onCastEnd);
        }
    }

    private void RF_Sharp_Claw(LivingEntity target, Action onCastEnd = null) //��ó�� �ο��ؼ� 3�� ���� 10�� ���ظ� ������.
    {
            SetIndicator(owner.gameObject, "����").OnEnd(() =>
            {
                target.GetDamage(10);

                Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
                effect.transform.position = owner.transform.position;
                effect.Play(() =>
                {
                    SetIndicator(owner.gameObject, "��ó �ο�").OnEnd(() =>
                    {
                        target.cc.SetCC(CCType.Wound, 3);
                        Anim_M_Butt effect = PoolManager.GetItem<Anim_M_Butt>();
                        effect.transform.position = owner.transform.position;
                        effect.Play(() =>
                        {
                            onCastEnd?.Invoke();
                        });
                    });
                });
            });
    }

    private void RF_Sneaky(LivingEntity target, Action onCastEnd = null) //�κ��丮�� '������ ����'�� 3�� �߰��Ѵ�.
    {
        SetIndicator(owner.gameObject, "����").OnEnd(() =>
        {
            target.GetDamage(20);

            Anim_M_Butt effect = PoolManager.GetItem<Anim_M_Butt>();
            effect.transform.position = owner.transform.position;
            effect.Play(() =>
            {
                SetIndicator(owner.gameObject, "���� �߰�").OnEnd(() =>
                {
                    Inventory owner1 = owner.GetComponent<EnemyInventory>();

                    for (int i = 0; i < 3; i++)
                    {
                        GameManager.Instance.inventoryHandler.CreateSkill(presentgSkill, owner1);
                    }
                });
                onCastEnd?.Invoke();
            });

        });
    }


}
