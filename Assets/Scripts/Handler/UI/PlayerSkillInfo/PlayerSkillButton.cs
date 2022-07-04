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
    public Sprite disableStroke;

    public CanvasGroup highlightCG;

    private Sequence highlightSeq;
    private Vector3 origin;

    private AnimHandler animHandler;

    public UseCountUI useCountUI;

    private void Awake()
    {
        skillBtn = GetComponent<Button>();
        origin = transform.localPosition;

        highlightCG.alpha = 0f;

        SetStrokeSprite(disableStroke);
    }

    private void Start()
    {
        animHandler = GameManager.Instance.animHandler;
    }

    public bool HasSkill()
    {
        return currentSkill != null;
    }

    public void Init(PlayerSkill skill, Action<PlayerSkill> onClickBtn)
    {
        useCountUI.gameObject.SetActive(false);

        if (currentSkill != null)
        {
            // 스킬 풀링
            currentSkill.gameObject.SetActive(false);
        }

        currentSkill = skill;

        skill.gameObject.SetActive(true);
        skill.transform.SetParent(btnPos);
        skill.transform.localScale = Vector3.one;
        skill.transform.localPosition = Vector3.zero;

        skill.Init(this);

        SetAddListener(() =>
        {
            onClickBtn?.Invoke(currentSkill);
        });
    }

    public void SetCurSkillAgain(Action<PlayerSkill> onClickBtn)
    {
        PlayerSkill skill = currentSkill;

        skill.gameObject.SetActive(true);
        skill.transform.SetParent(btnPos);
        skill.transform.localScale = Vector3.one;
        skill.transform.localPosition = Vector3.zero;

        skill.Init(this, true);

        SetAddListener(() =>
        {
            onClickBtn?.Invoke(currentSkill);
        });
    }

    public void SetAddListener(Action onClickBtn)
    {
        skillBtn.onClick.RemoveAllListeners();

        skillBtn.onClick.AddListener(() =>
        {
            onClickBtn?.Invoke();
        });
    }

    public void SetStrokeColor(Color color)
    {
        stroke.color = color;
    }

    public void SetStrokeSprite(Sprite spr)
    {
        stroke.sprite = spr;
    }

    public void ShowHighlight()
    {
        highlightSeq.Kill();
        highlightSeq = DOTween.Sequence()
            .Append(highlightCG.DOFade(0f, 0.4f).From(1f).SetEase(Ease.Linear))
            .Join(transform.DOShakePosition(0.25f, 40f, 50))
            .AppendCallback(() => 
            {
                transform.localPosition = origin;
            });

        if (currentSkill == null)
            return;

        ShowMessege($"{currentSkill.skillName} 준비됨!");
    }

    public void ShowMessege(string msg, TextUpAnimType animType = TextUpAnimType.Up)
    {
        animHandler.GetTextAnim()
            .SetPosition(highlightCG.transform.position)
            .SetScale(0.7f)
            .SetType(animType)
            .Play(msg);
    }

    public void SkillUseEffect()
    {
        if (currentSkill == null)
            return;

        ShowMessege($"{currentSkill.skillName} 사용!");
    }

    public void RemoveSkill()
    {
        skillBtn.onClick.RemoveAllListeners();

        useCountUI.Init(0);
        currentSkill.gameObject.SetActive(false);

        currentSkill = null;

        ShowMessege("스킬 삭제됨!", TextUpAnimType.Fixed);
        SetStrokeSprite(disableStroke);
    }
}
