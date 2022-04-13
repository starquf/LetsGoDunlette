using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class FieldHandler : MonoBehaviour
{
    public List<ParticleSystem> particles;
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