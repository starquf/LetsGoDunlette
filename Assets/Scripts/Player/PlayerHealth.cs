using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Color damageBGColor;
    public Color healBGColor;
    public Image damageBGEffect;
    public HealParticleSetter healParticle;

    public Text TopPanelHPText;

    public override void SetHPBar()
    {
        if (IsDie)
        {
            TopPanelHPText.text = $"»ç¸Á";
        }
        else
        {
            TopPanelHPText.text = $"{hp}/{maxHp}";
        }
        base.SetHPBar();
    }

    public override void GetDamage(int damage, bool isCritical = false)
    {
        base.GetDamage(damage);

        damageBGEffect.color = damageBGColor;
        damageBGEffect.DOFade(0f, 0.55f);
    }

    public override void Heal(int value)
    {
        base.Heal(value);

        GameManager.Instance.animHandler.GetAnim(AnimName.PlayerHeal).SetPosition(bh.mainRullet.transform.position)
            .SetScale(2.5f)
            .Play();
        healParticle.PLay(0.55f);
        damageBGEffect.color = healBGColor;
        damageBGEffect.DOFade(0f, 0.55f);
    }
}
