using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    private static Dictionary<string, IPool> poolDic = new Dictionary<string, IPool>();

    public static void CreatePool<T>(GameObject prefab, Transform parent, int count = 5) where T : MonoBehaviour
    {
        Type t = typeof(T);

        foreach (string key in poolDic.Keys)
        {
            if (key.Equals(t.ToString()))
            {
                return;
            }
        }

        ObjectPooling<T> pool = new ObjectPooling<T>(prefab, parent, count);

        poolDic.Add(t.ToString(), pool);
    }

    public static void CreateEnemyPool(EnemyType t, GameObject prefab, Transform parent, int count = 5)
    {
        foreach (string key in poolDic.Keys)
        {
            if (key.Equals(t.ToString()))
            {
                return;
            }
        }

        EnemyPooling pool = new EnemyPooling(prefab, parent, count);

        poolDic.Add(t.ToString(), pool);
    }

    public static T GetItem<T>() where T : MonoBehaviour
    {
        Type t = typeof(T);
        ObjectPooling<T> pool = (ObjectPooling<T>)poolDic[t.ToString()];
        return pool.GetOrCreate();
    }

    public static EnemyHealth GetEnemy(EnemyType t)
    {
        EnemyPooling pool = (EnemyPooling)poolDic[t.ToString()];
        return pool.GetOrCreate();
    }

    public static void ResetPool()
    {
        poolDic.Clear();
    }
}
