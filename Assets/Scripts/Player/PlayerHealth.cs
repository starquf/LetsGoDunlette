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

    protected override void Awake()
    {
        base.Awake();

        cc.isPlayer = true;
    }

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

        if (!GameManager.Instance.curEncounter.Equals(mapNode.RandomEncounter) && !GameManager.Instance.curEncounter.Equals(mapNode.REST))
        {
            GameManager.Instance.animHandler.GetAnim(AnimName.PlayerHeal).SetPosition(bh.mainRullet.transform.position)
                .SetScale(2.5f)
                .Play();

        }
        BuffParticleSetter bPS = null;
        bool hasEffect = GameManager.Instance.buffParticleHandler.otherParticleSetterDic.TryGetValue("Heal", out bPS);
        if (hasEffect)
        {
            bPS.Play(0.55f);
        }
    }
}
