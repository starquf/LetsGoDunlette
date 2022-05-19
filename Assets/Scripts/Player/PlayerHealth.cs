using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Color damageBGColor;
    public Color healBGColor;
    public Color ShieldBGColor;
    public Image damageBGEffect;
    public HealParticleSetter healParticle;
    public ShieldParticleSetter shieldParticle;

    public Text TopPanelHPText;

    public override void SetHPBar()
    {
        TopPanelHPText.text = IsDie ? $"»ç¸Á" : $"{hp}/{maxHp}";
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
        
        if(!GameManager.Instance.curEncounter.Equals(mapNode.RandomEncounter) && !GameManager.Instance.curEncounter.Equals(mapNode.REST))
        {
            GameManager.Instance.animHandler.GetAnim(AnimName.PlayerHeal).SetPosition(bh.mainRullet.transform.position)
                .SetScale(2.5f)
                .Play();

        }
        healParticle.Play(0.55f);
        damageBGEffect.color = healBGColor;
        damageBGEffect.DOFade(0f, 0.55f);
    }

    public override void AddShield(int value)
    {
        base.AddShield(value);

        StartCoroutine(ShieldEffect());
    }

    private IEnumerator ShieldEffect()
    {
        shieldParticle.Play(0.5f);

        yield return new WaitForSeconds(0.4f);
        damageBGEffect.color = ShieldBGColor;
        damageBGEffect.DOFade(0f, 0.55f);
    }
}
