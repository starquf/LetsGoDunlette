using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BuffParticleSetter : MonoBehaviour
{
    [HideInInspector] public Image damageBGEffect;
    [HideInInspector] public ParticleSystem buffParticle;

    public Color buffColor = new Color();
    public ParticleSetterType particleSetterType = ParticleSetterType.Other;
    public BuffType bType = BuffType.Shield;
    public CCType cType = CCType.Exhausted;
    public string otherName = "";

    protected virtual void Awake()
    {
        buffParticle = GetComponent<ParticleSystem>();
    }

    public virtual void Play(float time, float waitTime = 0)
    {
        StartCoroutine(PlayAnim(time, waitTime));
    }

    protected virtual IEnumerator PlayAnim(float time, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        damageBGEffect.color = buffColor;
        damageBGEffect.DOFade(0f, 0.55f);

        ParticleSystem.MainModule m = buffParticle.main;
        m.startLifetime = time;
        buffParticle.Play();
    }
}
