using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEffect : MonoBehaviour
{
    [Header("¿Ã∆Â∆Æ «¡∏Æ∆’")]
    public GameObject effectObj;

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        PoolManager.CreatePool<EffectObj>(effectObj, this.transform, 10);
    }
}
