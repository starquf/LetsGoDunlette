using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconInfo : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI iconName;
    [SerializeField] private TextMeshProUGUI iconDes;

    private CanvasGroup cvs;

    private Sequence highlightSeq;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();

        cvs.alpha = 0f;
    }

    public void Init(Sprite icon, string name, string des)
    {
        this.icon.sprite = icon;
        iconName.text = name;
        iconDes.text = des;
    }

    public void ShowHighlight(float delay)
    {
        highlightSeq.Kill();

        cvs.alpha = 0f;

        highlightSeq = DOTween.Sequence()
            .AppendInterval(delay)
            .Append(cvs.DOFade(1f, 0.3f));
    }

    public void ShowPanel(bool enable)
    {
        highlightSeq.Kill();

        cvs.alpha = enable ? 1f : 0f;
    }
}
