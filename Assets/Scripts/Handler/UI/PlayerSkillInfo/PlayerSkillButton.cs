using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillButton : MonoBehaviour
{
    private Button skillBtn;

    public Transform btnPos;
    public PlayerSkill currentSkill;

    [Header("UI")]
    public Image stroke;
    public CanvasGroup highlightCG;

    private Sequence highlightSeq;
    private Vector3 origin;

    private AnimHandler animHandler;

    private void Awake()
    {
        skillBtn = GetComponent<Button>();
        origin = transform.position;

        highlightCG.alpha = 0f;
    }

    private void Start()
    {
        animHandler = GameManager.Instance.animHandler;
    }

    public void Init(PlayerSkill skill, Action<PlayerSkill> onClickBtn)
    {
        currentSkill = skill;

        skill.Init(this);

        skillBtn.onClick.AddListener(() =>
        {
            onClickBtn?.Invoke(currentSkill);
        });
    }

    public void SetStrokeColor(Color color)
    {
        stroke.color = color;
    }

    public void ShowHighlight()
    {
        highlightSeq.Kill();
        highlightSeq = DOTween.Sequence()
            .Append(highlightCG.DOFade(0f, 0.4f).From(1f).SetEase(Ease.Linear))
            .Join(transform.DOShakePosition(0.25f, 40f, 50))
            .AppendCallback(() => 
            {
                transform.position = origin;
            });

        animHandler.GetTextAnim()
            .SetPosition(highlightCG.transform.position)
            .SetScale(0.7f)
            .SetType(TextUpAnimType.Up)
            .Play($"{currentSkill.skillName} ¡ÿ∫Òµ !");
    }
}
