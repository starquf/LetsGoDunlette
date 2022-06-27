using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPooling : IPool
{
    private Queue<PlayerSkill> m_queue;
    private GameObject prefab;
    private Transform parent;

    //�⺻ 5���� ���� ������ ������Ʈ Ǯ�� ������.
    public SkillPooling(GameObject prefab, Transform parent, int count = 5)
    {
        this.prefab = prefab;
        this.parent = parent;
        m_queue = new Queue<PlayerSkill>();

        for (int i = 0; i < count; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab, parent);
            PlayerSkill playerSkill = obj.GetComponent<PlayerSkill>();
            obj.SetActive(false);
            m_queue.Enqueue(playerSkill);
        }
    }

    public PlayerSkill GetOrCreate()
    {
        PlayerSkill playerSkill = m_queue.Peek();
        if (playerSkill.gameObject.activeSelf)
        {
            GameObject temp = GameObject.Instantiate(prefab, parent);
            playerSkill = temp.GetComponent<PlayerSkill>();
        }
        else
        {
            playerSkill = m_queue.Dequeue();
        }

        playerSkill.gameObject.SetActive(true);

        m_queue.Enqueue(playerSkill);
        return playerSkill;
    }
}
