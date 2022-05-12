using System;
using System.Collections.Generic;

public class NSL_Attack : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());

        return desInfos;
    }

    private int GetDamageCalc()
    {
        int attack = (int)(owner.GetComponent<LivingEntity>().AttackPower * 0.4f); // 0.4 ¸¦ ¿¢¼¿¿¡¼­ °¡Á®¿Í¾ßÇÔ ¿¢¼¿

        return attack;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(owner.gameObject, "°ø°Ý").OnEndAction(() =>
        {
            target.GetDamage(GetDamageCalc(), this,owner);
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

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
