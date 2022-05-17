using System.Collections.Generic;
using UnityEngine;

public class ScrollPooling : IPool
{
    private Queue<Scroll> m_queue;
    private GameObject prefab;
    private Transform parent;

    //�⺻ 5���� ���� ������ ������Ʈ Ǯ�� ������.
    public ScrollPooling(GameObject prefab, Transform parent, int count = 5)
    {
        this.prefab = prefab;
        this.parent = parent;
        m_queue = new Queue<Scroll>();

        for (int i = 0; i < count; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab, parent);
            Scroll scroll = obj.GetComponent<Scroll>();
            obj.SetActive(false);
            m_queue.Enqueue(scroll);
        }
    }

    public Scroll GetOrCreate()
    {
        Scroll scroll = m_queue.Peek();
        if (scroll.gameObject.activeSelf)
        {
            GameObject temp = GameObject.Instantiate(prefab, parent);
            scroll = temp.GetComponent<Scroll>();
        }
        else
        {
            scroll = m_queue.Dequeue();
        }

        scroll.gameObject.SetActive(true);

        m_queue.Enqueue(scroll);
        return scroll;
    }
}
