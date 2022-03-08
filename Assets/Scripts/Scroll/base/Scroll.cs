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


    [SerializeField]
    protected string scrollName;
    public string ScrollName => scrollName;

    [SerializeField]
    [TextArea]
    protected string scrollDes;
    public string ScrollDes => scrollDes;

    public ScrollSlot slot;

    private void Awake()
    {
        scrollIcon = GetComponent<Image>().sprite;
    }

    public abstract void Use(Action onEndUse, Action onCancelUse);
}
