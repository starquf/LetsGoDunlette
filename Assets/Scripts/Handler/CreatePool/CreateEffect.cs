using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEffect : MonoBehaviour
{
    [Header("������")]
    public GameObject effectObj;

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        //print("�����");
        PoolManager.CreatePool<EffectObj>(effectObj, this.transform, 10);
    }
}
