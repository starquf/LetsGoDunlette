using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateScroll : MonoBehaviour
{
    [Header("스크롤 오브젝트들")]
    public GameObject scroll_HealObj;
    public GameObject scroll_ShieldObj;
    public GameObject scroll_UseObj;
    public GameObject scroll_ChaosObj;
    public GameObject scroll_MemorieObj;

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        PoolManager.CreatePool<Scroll_Heal>(scroll_HealObj, this.transform, 2);
        PoolManager.CreatePool<Scroll_Shield>(scroll_ShieldObj, this.transform, 2);
        PoolManager.CreatePool<Scroll_Use>(scroll_UseObj, this.transform, 2);
        PoolManager.CreatePool<Scroll_Chaos>(scroll_ChaosObj, this.transform, 2);
        PoolManager.CreatePool<Scroll_Memorie>(scroll_MemorieObj, this.transform, 2);
    }
}
