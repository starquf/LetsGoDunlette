using System;
using UnityEngine;

public class HP_Gliding : SkillPiece
{
    public GameObject scratchingSkill; // ������
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "������ �߰�").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            animHandler.GetAnim(AnimName.SkillEffect01)
            .SetPosition(Owner.transform.position)
            .SetRotation(Vector3.forward * -90f)
            .SetScale(2f)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });

            GlidingSkill();

        });

    }

    //��ų �κ�
    public void GlidingSkill() //���� ������ 2�� ����ִ´�.
    {
        Inventory Owner1 = Owner.GetComponent<EnemyInventory>();

        for (int i = 0; i < value; i++)
        {
            bh.battleUtil.SetTimer(0.25f * i, () => { GameManager.Instance.inventoryHandler.CreateSkill(scratchingSkill, Owner1, Owner1.transform.position); });
        }
    }
}
