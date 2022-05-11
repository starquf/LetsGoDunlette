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
    private Color emptyColor = Color.red;

    private void Awake()
    {
        pieceText = GetComponentInChildren<Text>();

        originScale = transform.localScale;

        ColorUtility.TryParseHtmlString("#F03F59", out emptyColor);
    }

    public void SetText(int count)
    {
        if (count <= 0)
        {
            pieceText.color = emptyColor;
        }
        else
        {
            pieceText.color = Color.white;
        }

        pieceText.text = count.ToString();
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
