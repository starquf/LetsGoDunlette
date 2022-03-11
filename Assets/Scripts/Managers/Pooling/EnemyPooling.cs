using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooling : IPool
{
    private Queue<EnemyHealth> m_queue;
    private GameObject prefab;
    private Transform parent;

    //기본 5개의 값을 가지는 오브젝트 풀을 만들어낸다.
    public EnemyPooling(GameObject prefab, Transform parent, int count = 5)
    {
        this.prefab = prefab;
        this.parent = parent;
        m_queue = new Queue<EnemyHealth>();

        for (int i = 0; i < count; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab, parent);
            EnemyHealth enemy = obj.GetComponent<EnemyHealth>();
            obj.SetActive(false);
            m_queue.Enqueue(enemy);
        }
    }

    public EnemyHealth GetOrCreate()
    {
        EnemyHealth enemy = m_queue.Peek();
        if (enemy.gameObject.activeSelf)
        {
            GameObject temp = GameObject.Instantiate(prefab, parent);
            enemy = temp.GetComponent<EnemyHealth>();
        }
        else
        {
            enemy = m_queue.Dequeue();
        }

        enemy.gameObject.SetActive(true);

        m_queue.Enqueue(enemy);
        return enemy;
    }
}
