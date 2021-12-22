using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling<T> : IPool where T : MonoBehaviour
{
    private Queue<T> m_queue;
    private GameObject prefab;
    private Transform parent;

    //기본 5개의 값을 가지는 오브젝트 풀을 만들어낸다.
    public ObjectPooling(GameObject prefab, Transform parent, int count = 5)
    {
        this.prefab = prefab;
        this.parent = parent;
        m_queue = new Queue<T>();
        for(int i = 0; i < count; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab, parent);
            T t = obj.GetComponent<T>();
            obj.SetActive(false);
            m_queue.Enqueue(t);
        }
    }

    public T GetOrCreate()
    {
        T t = m_queue.Peek();
        if (t.gameObject.activeSelf)
        {
            GameObject temp = GameObject.Instantiate(prefab, parent);
            t = temp.GetComponent<T>();
        }
        else
        {
            t = m_queue.Dequeue();
        }

        t.gameObject.SetActive(true);

        m_queue.Enqueue(t);
        return t;
    }
}


