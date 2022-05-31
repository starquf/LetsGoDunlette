using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShieldParticleSetter : BuffParticleSetter
{
    [HideInInspector] public Image shieldAnimImage;

    private Color startShieldColor = new Color();

    protected override void Awake()
    {
        base.Awake();
        shieldAnimImage = GetComponentInChildren<Image>();
        startShieldColor = shieldAnimImage.color;
    }

    protected override IEnumerator PlayAnim(float time, float waitTime)
    {
        buffParticle.Play();

        yield return new WaitForSeconds(time * 0.9f);

        damageBGEffect.color = buffColor;
        damageBGEffect.DOFade(0f, 0.55f);

        shieldAnimImage.fillAmount = 0;
        shieldAnimImage.color = startShieldColor;

        DOTween.Sequence().Append(DOTween.To(() => shieldAnimImage.fillAmount, (x) => shieldAnimImage.fillAmount = x, 1, 0.5f))
            .Append(shieldAnimImage.DOColor(new Color(buffColor.r, buffColor.g, buffColor.b, 0f), 0.5f));
    }
}
