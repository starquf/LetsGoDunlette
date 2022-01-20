using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class PieceCastUIHandler : MonoBehaviour
{
    public Transform parent;

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
        pieceMoveSequence.Kill();
        skillPiece.transform.SetParent(parent);
        pieceMoveSequence = DOTween.Sequence().Append(skillPiece.transform.DOMove(parent.position, 0.5f))
            .Join(skillPiece.transform.DORotate(Quaternion.Euler(0, 0, 30).eulerAngles, 0.5f))
            .OnComplete(() => { print("ÀÌÆåÆ®³¡³²"); onEndEffect(); });

        ShowPanel(true);
    }

    public void EndCast(SkillPiece skillPiece)
    {
        ShowPanel(false);
    }

    public void ShowPanel(bool enable, bool skip = false)
    {
        showSequence.Kill();
        if (!skip)
        {
            showSequence = DOTween.Sequence().Append(cvsGroup.DOFade(enable ? 1 : 0, enable ? 0.2f:0.5f).OnComplete(() => {
                cvsGroup.interactable = enable;
                cvsGroup.blocksRaycasts = enable;
            }));
        }
        else
        {
            cvsGroup.alpha = enable ? 1 : 0;
            cvsGroup.interactable = enable;
            cvsGroup.blocksRaycasts = enable;
        }
    }
}
