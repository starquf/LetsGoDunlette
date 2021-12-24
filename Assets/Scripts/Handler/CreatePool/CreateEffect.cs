using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEffect : MonoBehaviour
{
    [Header("프리팹")]
    public GameObject effectObj;

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        //print("만들어");
        PoolManager.CreatePool<EffectObj>(effectObj, this.transform, 10);
    }
}
