using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public abstract class RulletPiece : MonoBehaviour
{
    public PatternType patternType = PatternType.None;

    public PieceType PieceType { get; protected set; }

    [SerializeField]
    protected int size;
    public int Size => size;

    [SerializeField]
    protected string pieceName;
    public string PieceName => pieceName;

    [SerializeField]
    [TextArea]
    protected string pieceDes;
    public string PieceDes => pieceDes;

    [SerializeField]
    protected int value;
    public int Value => value;

    private Image highlightImg;

    public Image skillImg;

    private float r;
    private Vector3 pos;

    protected virtual void Start()
    {
        highlightImg = GetComponentsInChildren<Image>()[1];
        transform.GetComponent<Image>().fillAmount = Size / 36f;

        if (skillImg == null) return;

        float angle = -360f * ((Size / 36f) / 2f);

        // 반지름
        r = transform.GetComponent<RectTransform>().rect.width / 3.6f;

        // 각도의 한 점 (방향 벡터)
        pos = new Vector3(Mathf.Cos((angle + 90f) * Mathf.Deg2Rad), Mathf.Sin((angle + 90f) * Mathf.Deg2Rad), 0f);

        skillImg.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        skillImg.transform.localPosition = pos * r;
    }

    public virtual void ChangePieceName(string pieceName)
    {
        this.pieceName = pieceName;
    }
    public virtual void ChangeValue(int value)
    {
        this.value = value;
    }

    public virtual void ChangeSize(int size)
    {
        this.size = Mathf.Clamp(size, 0, 36);
    }

    public virtual void AddSize(int size)
    {
        int result = this.size + size;

        this.size = Mathf.Clamp(result, 0, 36);
    }

    public void ResetPiece()
    {
        transform.localScale = Vector3.one;
        skillImg.transform.localScale = Vector3.one;
        highlightImg.color = Color.clear;
    }

    public virtual void Highlight()
    {
        highlightImg.color = Color.white;
        highlightImg.DOFade(0f, 0.5f)
            .SetEase(Ease.InQuad);

        skillImg.transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.5f);
    }

    public virtual void HighlightColor(float dur)
    {
        highlightImg.color = Color.white;
        highlightImg.DOFade(0f, dur)
            .SetEase(Ease.InQuad);
    }

    public void UnHighlight()
    {
        skillImg.transform.DOScale(Vector3.one, 0.5f);
    }

    public abstract void Cast(Action onCastEnd = null);
}
