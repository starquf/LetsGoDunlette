using DG.Tweening;
using UnityEngine;

public class E_FieldHandler : FieldHandler
{
    [SerializeField]
    private SpriteRenderer redTransSpr;

    private Color redTransColor;
    private void Awake()
    {
        redTransColor = redTransSpr.color;

        fieldType = ElementalType.Electric;
    }
    public override void DisableField(bool skip = false)
    {
        base.DisableField();
        if (skip)
        {
            redTransSpr.color = new Color(redTransColor.r, redTransColor.g, redTransColor.b, 0);
        }
        else
        {
            redTransSpr.DOKill();
            redTransSpr.DOFade(0, 0.5f);
        }
    }

    public override void EnableField(bool skip = false)
    {
        base.EnableField();
        if (skip)
        {
            redTransSpr.color = redTransColor;
        }
        else
        {
            redTransSpr.DOKill();
            redTransSpr.DOFade(redTransColor.a, 0.5f);
        }
    }
}
