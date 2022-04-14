using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceCastUIHandler : MonoBehaviour
{
    public Transform parent;

    [Header("Ä«µå UI")]
    public Image cardBG;
    public Text cardNameText;
    public Text cardDesText;
    public Button closeBtn;

    private CanvasGroup cvsGroup;
    private Sequence showSequence;
    private Sequence pieceMoveSequence;

    [Header("»ö±òµé")]
    public List<Color> colors = new List<Color>();
    public Dictionary<PatternType, Color> colorDic;

    void Awake()
    {
        cvsGroup = GetComponent<CanvasGroup>();

        ShowPanel(false, true);

        colorDic = new Dictionary<PatternType, Color>();
        for (int i = 0; i < colors.Count; i++)
        {
            colorDic.Add((PatternType)i, colors[i]);
        }
    }

    public void CastSkill(SkillPiece skillPiece, LivingEntity targetTrm, Action onCastEnd = null)
    {
        skillPiece.Cast(targetTrm, onCastEnd);
    }

    public void ShowCasting(SkillPiece skillPiece, Action onEndEffect)
    {
        if (skillPiece.isRandomSkill)
        {
            PieceInfo info = skillPiece.ChoiceSkill();
            cardBG.sprite = skillPiece.cardBG;
            cardNameText.text = info.PieceName;
            cardDesText.text = info.PieceDes;
        }
        else
        {
            cardBG.sprite = skillPiece.cardBG;
            cardNameText.text = skillPiece.PieceName;
            cardDesText.text = skillPiece.PieceDes;
        }

        pieceMoveSequence.Kill();

        skillPiece.gameObject.SetActive(true);
        skillPiece.transform.SetParent(parent);

        pieceMoveSequence = DOTween.Sequence()
            .Append(skillPiece.transform.DOMove(parent.position, 0.5f))
            .Join(skillPiece.transform.DORotate(Quaternion.Euler(0, 0, 30).eulerAngles, 0.5f))
            .InsertCallback(0.25f, () =>
            {
                Anim_SkillDetermined effect = PoolManager.GetItem<Anim_SkillDetermined>();

                effect.transform.position = skillPiece.skillImg.transform.position;
                effect.SetRotation(skillPiece.skillImg.transform.eulerAngles);
                effect.SetScale(1.1f);
                effect.ChangeColor(colorDic[skillPiece.patternType]);

                effect.Play();
            })
            //.Join(skillPiece.transform.DOScale(Vector3.one, 0.5f))
            .AppendInterval(0.3f)
            .Append(skillPiece.GetComponent<Image>().DOFade(0, 0.3f))
            .Join(skillPiece.skillImg.DOFade(0, 0.3f))
            .OnComplete(() =>
            { //print("ÀÌÆåÆ®³¡³²");
                skillPiece.gameObject.SetActive(false);

                SetCloseBtn(() => 
                {
                    onEndEffect();
                });
            });

        ShowPanel(true);
    }

    public void ShowCasting(PieceInfo info, Action onEndEffect)
    {
        cardBG.sprite = info.cardBG;
        cardNameText.text = info.PieceName;
        cardDesText.text = info.PieceDes;

        pieceMoveSequence.Kill();

        SetCloseBtn(() =>
        {
            onEndEffect();
        });

        ShowPanel(true);
    }

    public void EndCast(SkillPiece skillPiece)
    {
        ShowPanel(false, false, () =>
        {
            if (skillPiece != null)
            {
                skillPiece.GetComponent<Image>().color = Color.white;
                skillPiece.skillImg.color = Color.white;
            }
        });
    }

    public void SetCloseBtn(Action action)
    {
        closeBtn.onClick.AddListener(() =>
        {
            action.Invoke();
            closeBtn.onClick.RemoveAllListeners();
        });
    }

    public void ShowPanel(bool enable, bool skip = false, Action endEvent = null)
    {
        showSequence.Kill();
        if (!skip)
        {
            showSequence = DOTween.Sequence().Append(cvsGroup.DOFade(enable ? 1 : 0, enable ? 0.2f : 0.3f).OnComplete(() =>
            {
                cvsGroup.interactable = enable;
                cvsGroup.blocksRaycasts = enable;
                endEvent?.Invoke();
            }));
        }
        else
        {
            cvsGroup.alpha = enable ? 1 : 0;
            cvsGroup.interactable = enable;
            cvsGroup.blocksRaycasts = enable;
            endEvent?.Invoke();
        }
    }
}
