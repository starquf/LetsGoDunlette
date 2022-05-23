using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    public override void Play(float time)
    {
        base.Play(time);

        StartCoroutine(PlayAnim(time));
    }

    public IEnumerator PlayAnim(float time)
    {
        yield return new WaitForSeconds(time*0.9f);

        damageBGEffect.color = buffColor;
        damageBGEffect.DOFade(0f, 0.55f);

        shieldAnimImage.fillAmount = 0;
        shieldAnimImage.color = startShieldColor;

        DOTween.Sequence().Append(DOTween.To(() => shieldAnimImage.fillAmount, (x) => shieldAnimImage.fillAmount = x, 1, 0.5f))
            .Append(shieldAnimImage.DOColor(buffColor, 0.5f));
    }
}
