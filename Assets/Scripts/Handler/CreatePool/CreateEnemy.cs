using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{
    [Header("적 오브젝트들")]
    public List<EnemyHealth> enemyList = new List<EnemyHealth>();

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            PoolManager.CreateEnemyPool(enemyList[i].enemyType, enemyList[i].gameObject, this.transform, 1);
        }
    }
}
