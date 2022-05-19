using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealParticleSetter : MonoBehaviour
{
    [HideInInspector] public ParticleSystem healParticle;

    private void Awake()
    {
        healParticle = GetComponent<ParticleSystem>();
    }

    public void PLay(float time)
    {
        //var mainModule = healParticle.main;
        //mainModule.startLifetime = time;
        //healParticle.startLifetime = time;
        ParticleSystem.MainModule m = healParticle.main;
        m.startLifetime = time;
        healParticle.Play();
    }
}
