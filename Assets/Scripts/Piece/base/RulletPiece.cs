using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PieceInfo
{
    public string PieceName;
    public string PieceDes;
    public Sprite cardBG;
    public List<int> value;

    public PieceInfo(string pieceName, string pieceDes)
    {

        PieceName = pieceName;
        PieceDes = pieceDes;
    }

    public int GetValue(int index = 0)
    {
        return value[index];
    }
}

[Serializable]
public class DesIconInfo
{
    public DesIconType iconType = DesIconType.None;
    public string value;

    public void SetInfo(DesIconType type, string msg)
    {
        iconType = type;
        value = msg;
    }
}

public abstract class RulletPiece : MonoBehaviour
{
    public ElementalType patternType = ElementalType.None;
    public ElementalType currentType = ElementalType.None;

    public PieceType PieceType { get; protected set; }

    protected int size = 6;
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

    protected Image bgImg;
    protected Image highlightImg;

    [HideInInspector]
    public Image skillIconImg;

    private Transform skillIconTrans;

    [HideInInspector]
    public int pieceIdx = 0;

    [Header("카드 배경")]
    public Sprite cardBG;

    [HideInInspector]
    public Sprite skillStroke;

    private float r;
    private Vector3 pos;

    protected virtual void Awake()
    {
        currentType = patternType;

        bgImg = GetComponent<Image>();
        highlightImg = GetComponentsInChildren<Image>()[1];

        highlightImg.fillAmount = Size / 36f;
        transform.GetComponent<Image>().fillAmount = Size / 36f;

        skillIconTrans = transform.Find("SkillIcons");
        if (skillIconTrans == null)
        {
            print("스킬 아이콘이 없음 !!");
            return;
        }

        skillIconImg = skillIconTrans.Find("SkillBG").Find("Icon").GetComponent<Image>();
        if (skillIconImg == null)
        {
            return;
        }

        skillIconImg.sprite = cardBG;
        skillStroke = skillIconTrans.Find("SkillStoke").GetComponent<Image>().sprite;

        float angle = -360f * (Size / 36f / 2f);

        // 반지름
        r = transform.GetComponent<RectTransform>().rect.width / 3.6f;

        // 각도의 한 점 (방향 벡터)
        pos = new Vector3(Mathf.Cos((angle + 90f) * Mathf.Deg2Rad), Mathf.Sin((angle + 90f) * Mathf.Deg2Rad), 0f);

        skillIconTrans.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        skillIconTrans.transform.localPosition = pos * r;
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

    public virtual void ChangeType(ElementalType type)
    {
        if (currentType != type)
        {
            bgImg.sprite = GameManager.Instance.inventoryHandler.pieceBGSprDic[type];
        }

        currentType = type;
    }

    public virtual void AddSize(int size)
    {
        int result = this.size + size;

        this.size = Mathf.Clamp(result, 0, 36);
    }

    public virtual void ResetPiece()
    {
        KillTween();

        pieceIdx = 0;
        transform.localScale = Vector3.one;
        skillIconImg.transform.localScale = Vector3.one;
        skillIconImg.color = Color.white;
        bgImg.color = Color.white;
        highlightImg.color = Color.clear;

        ChangeType(patternType);
    }

    public virtual void OnRullet()
    {

    }

    public void KillTween()
    {
        if (transform == null)
        {
            return;
        }

        transform.DOKill();
        skillIconImg.transform.DOKill();
        bgImg.DOKill();
        highlightImg.DOKill();
    }

    public virtual void Highlight()
    {
        highlightImg.color = Color.white;
        highlightImg.DOFade(0f, 0.5f)
            .SetEase(Ease.InQuad);

        skillIconImg.transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.5f);
    }

    public virtual void HighlightColor(float dur)
    {
        highlightImg.color = Color.white;
        highlightImg.DOFade(0f, dur)
            .SetEase(Ease.InQuad);
    }

    public void UnHighlight()
    {
        skillIconImg.transform.DOScale(Vector3.one, 0.5f);
    }

    public void AddValue(int value)
    {
        this.value += value;
    }

    public void MinusValue(int value)
    {
        this.value -= value;
    }

    public void SetValue(int value)
    {
        this.value = value;
    }

    public abstract void Cast(LivingEntity target, Action onCastEnd = null);

    protected virtual void OnDestroy()
    {
        KillTween();
    }
}
