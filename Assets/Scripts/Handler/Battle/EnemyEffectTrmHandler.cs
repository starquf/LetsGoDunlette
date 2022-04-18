using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffectTrmHandler : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.enemyEffectTrm = this.transform;
    }
}
