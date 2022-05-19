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
        ParticleSystemRenderer healParticleSystemRenderer = null;
        healParticleSystemRenderer = GetComponent<ParticleSystemRenderer>();

        if (GameManager.Instance.curEncounter.Equals(mapNode.RandomEncounter) || GameManager.Instance.curEncounter.Equals(mapNode.REST))
        {
            healParticleSystemRenderer.sortingLayerName = "Effect";
            healParticleSystemRenderer.sortingOrder = 0;
        }
        else
        {
            healParticleSystemRenderer.sortingLayerName = "DownUI";
            healParticleSystemRenderer.sortingOrder = 11;
        }

        ParticleSystem.MainModule m = healParticle.main;
        m.startLifetime = time;
        healParticle.Play();
    }
}
