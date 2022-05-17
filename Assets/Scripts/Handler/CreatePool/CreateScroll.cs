using System.Collections.Generic;
using UnityEngine;

public class CreateScroll : MonoBehaviour
{
    [Header("스크롤 오브젝트들")]
    public List<Scroll> scrollList = new List<Scroll>();

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < scrollList.Count; i++)
        {
            PoolManager.CreateScrollPool(scrollList[i].scrollType, scrollList[i].gameObject, transform, 1);
        }
    }
}
