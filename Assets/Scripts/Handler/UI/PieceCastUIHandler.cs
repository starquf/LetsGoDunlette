using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class PieceCastUIHandler : MonoBehaviour
{
    public Transform parent;

    [Header("카드 UI")]
    public Image cardBG;
    public Text cardNameText;
    public Text cardDesText;

    private CanvasGroup cvsGroup;
    private Sequence showSequence;
    private Sequence pieceMoveSequence;

    void Awake()
    {
        cvsGroup = GetComponent<CanvasGroup>();

        ShowPanel(false, true);
    }

    public void CastSkill(SkillPiece skillPiece, LivingEntity targetTrm, Action onCastEnd = null)
    {
        skillPiece.Cast(targetTrm, onCastEnd);
    }

    public void ShowCasting(SkillPiece skillPiece, Action onEndEffect)
    {
        cardBG.sprite = skillPiece.cardBG;
        cardNameText.text = skillPiece.PieceName;
        cardDesText.text = skillPiece.PieceDes;

        pieceMoveSequence.Kill();
        skillPiece.transform.SetParent(parent);
        pieceMoveSequence = DOTween.Sequence()
            .Append(skillPiece.transform.DOMove(parent.position, 0.5f))
            .Join(skillPiece.transform.DORotate(Quaternion.Euler(0, 0, 30).eulerAngles, 0.5f))
            //.Join(skillPiece.transform.DOScale(Vector3.one, 0.5f))
            .AppendInterval(0.1f)
            .Append(skillPiece.GetComponent<Image>().DOFade(0, 0.3f))
            .Join(skillPiece.skillImg.DOFade(0, 0.3f))
            .OnComplete(() => { print("이펙트끝남"); onEndEffect(); });

        ShowPanel(true);
    }

    public void EndCast(SkillPiece skillPiece)
    {
        ShowPanel(false,false, ()=> {
            skillPiece.GetComponent<Image>().color = Color.white;
            skillPiece.skillImg.color = Color.white;
        });
    }

    public void ShowPanel(bool enable, bool skip = false, Action endEvent = null)
    {
        showSequence.Kill();
        if (!skip)
        {
            showSequence = DOTween.Sequence().Append(cvsGroup.DOFade(enable ? 1 : 0, enable ? 0.2f:0.5f).OnComplete(() => {
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
