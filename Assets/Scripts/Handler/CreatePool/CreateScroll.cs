using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateScroll : MonoBehaviour
{
    [Header("��ũ�� ������Ʈ��")]
    public GameObject scroll_HealObj;
    public GameObject scroll_ShieldObj;

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        PoolManager.CreatePool<Scroll_Heal>(scroll_HealObj, this.transform, 2);
        PoolManager.CreatePool<Scroll_Shield>(scroll_ShieldObj, this.transform, 2);
    }
}
