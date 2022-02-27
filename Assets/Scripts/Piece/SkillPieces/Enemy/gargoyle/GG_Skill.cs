using System;
using Random = UnityEngine.Random;

public class GG_Skill : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        if (Random.Range(1, 100) <= value)
        {
            GG_Beat(target, onCastEnd);
        }
        else
        {
            GG_Recover(target, onCastEnd);
        }
    }

    private void GG_Beat(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "����").OnComplete(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            target.GetDamage(30, owner.gameObject);

            Anim_M_Sword hitEffect = PoolManager.GetItem<Anim_M_Sword>();
            hitEffect.transform.position = owner.transform.position;

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });



    }

    private void GG_Recover(LivingEntity target, Action onCastEnd = null) //�ڽ��� ü���� 30��ŭ ȸ���Ѵ�.
    {
        SetIndicator(owner.gameObject, "����").OnComplete(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            owner.GetComponent<EnemyHealth>().Heal(30);

            Anim_M_Recover effect = PoolManager.GetItem<Anim_M_Recover>();
            effect.transform.position = owner.transform.position;

            effect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });

    }


}
