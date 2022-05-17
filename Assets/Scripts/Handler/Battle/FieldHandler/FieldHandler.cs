using System.Collections.Generic;
using UnityEngine;

public abstract class FieldHandler : MonoBehaviour
{
    public List<ParticleSystem> particles;

    [HideInInspector]
    public ElementalType fieldType;

    public virtual void EnableField(bool skip = false)
    {
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Play();
        }
    }
    public virtual void DisableField(bool skip = false)
    {
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Pause();
            particles[i].Clear();
        }
    }
}
