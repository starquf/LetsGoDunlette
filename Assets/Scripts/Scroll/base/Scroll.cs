using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Scroll : MonoBehaviour
{
    [HideInInspector]
    public ScrollType scrollType;
    [HideInInspector]
    public Sprite scrollIcon;

    private void Awake()
    {
        scrollIcon = GetComponent<Image>().sprite;
    }

    public abstract void Use(Action onEndUse);
}
