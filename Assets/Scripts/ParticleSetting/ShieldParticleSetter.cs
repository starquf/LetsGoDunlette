using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShieldParticleSetter : MonoBehaviour
{
    [HideInInspector] public ParticleSystem shieldParticle;
    [HideInInspector] public Image shieldAnimImage;

    public Color shieldColor = new Color();
    private Color startShieldColor = new Color();

    void Awake()
    {
        shieldParticle = GetComponent<ParticleSystem>();
        shieldAnimImage = GetComponentInChildren<Image>();
        startShieldColor = shieldAnimImage.color;
    }

    public void Play(float time)
    {
        //var mainModule = healParticle.main;
        //mainModule.startLifetime = time;
        //healParticle.startLifetime = time;
        ParticleSystemRenderer healParticleSystemRenderer = null;
        healParticleSystemRenderer = GetComponent<ParticleSystemRenderer>();

        ParticleSystem.MainModule m = shieldParticle.main;
        m.startLifetime = time;
        shieldParticle.Play();

        StartCoroutine(PlayAnim(time));
    }

    public IEnumerator PlayAnim(float time)
    {
        yield return new WaitForSeconds(time*0.9f);

        shieldAnimImage.fillAmount = 0;
        shieldAnimImage.color = startShieldColor;

        DOTween.Sequence().Append(DOTween.To(() => shieldAnimImage.fillAmount, (x) => shieldAnimImage.fillAmount = x, 1, 0.5f))
            .Append(shieldAnimImage.DOColor(shieldColor, 0.5f));
    }
}
