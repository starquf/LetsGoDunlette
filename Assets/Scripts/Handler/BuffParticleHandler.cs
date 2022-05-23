using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ParticleSetterType

{
    Other = 0,
    Buff = 1,
    CC = 2,
}

public class BuffParticleHandler : MonoBehaviour
{
    public Image damageBGEffect;
    public Transform buffParticleParentTrm;

    [HideInInspector] public List<BuffParticleSetter> otherParticleSetterList;
    [HideInInspector] public Dictionary<BuffType, BuffParticleSetter> buffParticleSetterDic;
    [HideInInspector] public Dictionary<CCType, BuffParticleSetter> ccParticleSetterDic;

    private void Awake()
    {
        GameManager.Instance.buffParticleHandler = this;

        buffParticleSetterDic = new Dictionary<BuffType, BuffParticleSetter>();
        ccParticleSetterDic = new Dictionary<CCType, BuffParticleSetter>();

        otherParticleSetterList = new List<BuffParticleSetter>(buffParticleParentTrm.GetComponentsInChildren<BuffParticleSetter>());

        for (int i = otherParticleSetterList.Count - 1; i >= 0; i--)
        {
            BuffParticleSetter buffParticleSetter = otherParticleSetterList[i];
            buffParticleSetter.damageBGEffect = damageBGEffect;
            switch (buffParticleSetter.particleSetterType)
            {
                case ParticleSetterType.Other:
                    break;
                case ParticleSetterType.Buff:
                    otherParticleSetterList.Remove(buffParticleSetter);
                    buffParticleSetterDic.Add(buffParticleSetter.bType, buffParticleSetter);
                    print(buffParticleSetter.bType + "세팅");
                    break;
                case ParticleSetterType.CC:
                    otherParticleSetterList.Remove(buffParticleSetter);
                    ccParticleSetterDic.Add(buffParticleSetter.cType, buffParticleSetter);
                    print(buffParticleSetter.cType + "세팅");
                    break;
                default:
                    break;
            }
        }
    }
}
