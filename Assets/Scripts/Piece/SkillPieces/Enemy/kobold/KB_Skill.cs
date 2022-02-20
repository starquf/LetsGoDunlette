using System;
using Random = UnityEngine.Random;

public class KB_Skill : SkillPiece
{
    private int stolenGolds = 0;
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        if (Random.Range(1, 100) <= value)
        {
            KB_Pickpocket(target, onCastEnd);
        }
        else
        {
            KB_Transform(target, onCastEnd);
        }
    }

    private void KB_Pickpocket(LivingEntity target, Action onCastEnd = null) //�÷��̾�Լ� 10��带 ��ģ��.
    {
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

        Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
        effect.transform.position = owner.transform.position;

        target.GetDamage(10, owner.gameObject);

        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });

        //��� ��ġ�� ��ƾ
        stolenGolds += 10;
    }

    private void KB_Transform(LivingEntity target, Action onCastEnd = null) //�̹� �������� �÷��̾�Լ� ��ģ ��常ŭ �������� �ش�.
    {
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

        Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
        effect.transform.position = owner.transform.position;

        target.GetDamage(stolenGolds, owner.gameObject);

        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }


}
