using System;
using Random = UnityEngine.Random;

public class NSL_Skill : SkillPiece
{
    [UnityEngine.Header("스킬 벨류")]
    public int BounceDmg = 25;
    public int recoverVal = 30;

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
            onCastSkill = NSl_Recover;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = NSL_Bounce;
            return pieceInfo[1];
        }
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void NSl_Recover(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "회복").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            animHandler.GetAnim(AnimName.M_Recover).SetPosition(Owner.transform.position)
            .SetScale(1)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });

            Owner.GetComponent<EnemyHealth>().Heal(recoverVal);
        });
    }

    private void NSL_Bounce(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            target.GetDamage(BounceDmg, this, Owner);

            animHandler.GetAnim(AnimName.M_Butt)
            .SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2f)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }


}
