using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryIndicator : MonoBehaviour
{
    private Text pieceText;

    private Tween effectTween;

    private Vector3 originScale;

    private void Awake()
    {
        pieceText = GetComponentInChildren<Text>();

        originScale = transform.localScale;
    }

    public void SetText(string text)
    {
        pieceText.text = text;
    }

    public virtual void ShowEffect()
    {
        effectTween.Kill();
        effectTween = transform.DOScale(originScale * 1.5f, 0.15f)
            .SetDelay(0.05f)
            .OnComplete(() =>
            {
                transform.DOScale(originScale, 0.15f);
            });
    }
}
