using System;
using Random = UnityEngine.Random;

public class LSW_Skill : SkillPiece
{
    [UnityEngine.Header("스킬 벨류")]
    public int cuttingDmg = 25;
    public int shieldVal = 15;


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
            onCastSkill = LSW_Cutting;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = LSW_Old_Shield;
            return pieceInfo[1];
        }
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void LSW_Cutting(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "공격").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            target.GetDamage(cuttingDmg, this, owner);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    private void LSW_Old_Shield(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "보호막").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            animHandler.GetAnim(AnimName.M_Shield).SetPosition(owner.transform.position)
            .SetScale(1)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });

            owner.GetComponent<EnemyHealth>().AddShield(shieldVal);
        });
    }


}
