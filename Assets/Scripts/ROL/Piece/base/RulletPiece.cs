using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class RulletPiece : MonoBehaviour
{
    [SerializeField] 
    protected int size;
    public int Size => size;

    [SerializeField] 
    protected string pieceName;
    public string PieceName => pieceName;

    [SerializeField]
    protected int value;
    public int Value => value;

    [SerializeField]
    protected Color color;
    public Color Color => color;

    private Image highlightImg;

    private void Start()
    {
        highlightImg = GetComponentsInChildren<Image>()[1];
        transform.GetComponent<Image>().fillAmount = Size / 36f;
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

    public virtual void Highlight()
    {
        highlightImg.color = Color.white;
        highlightImg.DOFade(0f, 0.5f)
            .SetEase(Ease.InQuad);
    }

    public abstract int Cast();
}
