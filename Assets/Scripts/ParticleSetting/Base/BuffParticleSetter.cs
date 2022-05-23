using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BuffParticleSetter : MonoBehaviour
{
    [HideInInspector] public Image damageBGEffect;
    [HideInInspector] public ParticleSystem buffParticle;

    public Color buffColor = new Color();
    private Color startShieldColor = new Color();
    public ParticleSetterType particleSetterType = ParticleSetterType.Other;
    public BuffType bType = BuffType.Shield;
    public CCType cType = CCType.Exhausted;

    protected virtual void Awake()
    {
        buffParticle = GetComponent<ParticleSystem>();
    }

    public virtual void Play(float time)
    {
        ParticleSystem.MainModule m = buffParticle.main;
        m.startLifetime = time;
        buffParticle.Play();
    }
}
