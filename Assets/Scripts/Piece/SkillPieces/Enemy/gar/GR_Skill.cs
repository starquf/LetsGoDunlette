using System;
using Random = UnityEngine.Random;

public class GR_Skill : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        GR_StrangeLight(target, onCastEnd);
    }

    private void GR_StrangeLight(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "공격").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);


            if (owner.GetComponent<EnemyHealth>().GetHpRatio() >= 50)
            {
                if (Random.Range(0, 100) < 40)
                {
                    animHandler.GetTextAnim()
                    .SetType(TextUpAnimType.Fixed)
                    .SetPosition(target.transform.position)
                    .SetScale(0.7f)
                    .Play("기절!");

                    target.cc.SetCC(CCType.Stun, 1);
                }
            }

            target.GetDamage(Value, this, owner);
            animHandler.GetAnim(AnimName.BuffEffect03).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(1.5f)
            .Play(() =>
           {
               onCastEnd?.Invoke();
           });
        });
    }
}
