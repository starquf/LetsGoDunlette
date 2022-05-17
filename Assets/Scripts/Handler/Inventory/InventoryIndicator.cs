using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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
        pieceText.color = count <= 0 ? emptyColor : Color.white;

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
