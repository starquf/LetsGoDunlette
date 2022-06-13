using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetIcon : MonoBehaviour
{
    private Button btn;
    private Image img;

    private void Awake()
    {
        btn = GetComponent<Button>();
        img = transform.GetChild(0).GetComponent<Image>();
    }

    public void Init(Sprite icon, Action onClickIcon)
    {
        btn.onClick.RemoveAllListeners();

        img.sprite = icon;
        btn.onClick.AddListener(() => { onClickIcon?.Invoke(); });
    }
}
