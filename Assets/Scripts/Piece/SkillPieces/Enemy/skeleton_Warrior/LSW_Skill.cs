using System;
using Random = UnityEngine.Random;

public class LSW_Skill : SkillPiece
{
    [UnityEngine.Header("스킬 벨류")]
    public int cuttingDmg = 25;
    public int shieldVal = 15;

    public PieceInfo[] pieceInfo;
    private Action<LivingEntity, Action> onCastSkill;

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

            Anim_M_Sword hitEffect = PoolManager.GetItem<Anim_M_Sword>();
            hitEffect.transform.position = GameManager.Instance.enemyEffectTrm.position; hitEffect.SetScale(2);

            hitEffect.Play(() =>
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

            Anim_M_Shield shieldEffect = PoolManager.GetItem<Anim_M_Shield>();
            shieldEffect.transform.position = owner.transform.position;

            shieldEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });

            owner.GetComponent<EnemyHealth>().AddShield(shieldVal);
        });
    }


}
