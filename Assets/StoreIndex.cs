using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreIndex : MonoBehaviour
{
    public int index = -1;

    private void Awake()
    {
        var item = FindObjectsOfType<StoreIndex>();
        if (item.Length > 1)
            Destroy(gameObject);
        DontDestroyOnLoad(this);
    }
}
