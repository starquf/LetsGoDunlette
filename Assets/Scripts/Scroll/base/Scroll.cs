using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Scroll : MonoBehaviour
{
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

    protected BattleHandler bh;

    private void Awake()
    {
        scrollIcon = GetComponent<Image>().sprite;
        bh = GameManager.Instance.battleHandler;
    }

    public abstract void Use(Action onEndUse, Action onCancelUse);
}
