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

    [HideInInspector] public Dictionary<string, BuffParticleSetter> otherParticleSetterDic;
    [HideInInspector] public Dictionary<BuffType, BuffParticleSetter> buffParticleSetterDic;
    [HideInInspector] public Dictionary<CCType, BuffParticleSetter> ccParticleSetterDic;

    private void Awake()
    {
        GameManager.Instance.buffParticleHandler = this;

        List<BuffParticleSetter> buffParticleSetterList = new List<BuffParticleSetter>(buffParticleParentTrm.GetComponentsInChildren<BuffParticleSetter>());

        buffParticleSetterDic = new Dictionary<BuffType, BuffParticleSetter>();
        ccParticleSetterDic = new Dictionary<CCType, BuffParticleSetter>();
        otherParticleSetterDic = new Dictionary<string, BuffParticleSetter>();

        for (int i = buffParticleSetterList.Count - 1; i >= 0; i--)
        {
            BuffParticleSetter buffParticleSetter = buffParticleSetterList[i];
            buffParticleSetter.damageBGEffect = damageBGEffect;
            buffParticleSetterList.Remove(buffParticleSetter);
            switch (buffParticleSetter.particleSetterType)
            {
                case ParticleSetterType.Other:
                    otherParticleSetterDic.Add(buffParticleSetter.otherName, buffParticleSetter);
                    break;
                case ParticleSetterType.Buff:
                    buffParticleSetterDic.Add(buffParticleSetter.bType, buffParticleSetter);
                    print(buffParticleSetter.bType + "세팅");
                    break;
                case ParticleSetterType.CC:
                    ccParticleSetterDic.Add(buffParticleSetter.cType, buffParticleSetter);
                    print(buffParticleSetter.cType + "세팅");
                    break;
                default:
                    break;
            }
        }
        buffParticleSetterList.Clear();
    }
}
