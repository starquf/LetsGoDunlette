using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Static : MonoBehaviour
{
    private bool isEnd = false;

    private void Awake()
    {
        isEnd = false;
    }

    public void EndEffect()
    {
        isEnd = true;
    }

    public bool IsEndEffect()
    {
        return isEnd;
    }
}
