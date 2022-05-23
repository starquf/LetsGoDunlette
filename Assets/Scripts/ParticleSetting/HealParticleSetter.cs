using UnityEngine;

public class HealParticleSetter : BuffParticleSetter
{
    public override void Play(float time)
    {

        ParticleSystemRenderer healParticleSystemRenderer = GetComponent<ParticleSystemRenderer>();

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

        base.Play(time);
    }
}
