using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEffect : MonoBehaviour
{
    [Header("����Ʈ ������")]
    public GameObject effectObj;

    [Header("UI ������")]
    public GameObject enemyIndicator;

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        PoolManager.CreatePool<EffectObj>(effectObj, this.transform, 10);

        if (enemyIndicator != null)
            PoolManager.CreatePool<EnemyIndicatorText>(enemyIndicator, this.transform, 10);
    }
}
