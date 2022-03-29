using System;
using Random = UnityEngine.Random;

public class GG_Skill : SkillPiece
{
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
            onCastSkill = GG_Beat;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = GG_Recover;
            return pieceInfo[1];
        }
    }


    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void GG_Beat(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "공격").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            target.GetDamage(30, this, owner);

            Anim_M_Sword hitEffect = PoolManager.GetItem<Anim_M_Sword>();
            hitEffect.transform.position = GameManager.Instance.enemyEffectTrm.position; hitEffect.SetScale(2);

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });



    }

    private void GG_Recover(LivingEntity target, Action onCastEnd = null) //자신의 체력을 30만큼 회복한다.
    {
        SetIndicator(owner.gameObject, "공격").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            owner.GetComponent<EnemyHealth>().Heal(30);

            Anim_M_Recover effect = PoolManager.GetItem<Anim_M_Recover>();
            effect.transform.position = GameManager.Instance.enemyEffectTrm.position; effect.SetScale(2);

            effect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });

    }


}
