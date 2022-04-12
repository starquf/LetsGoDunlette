using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

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
            .AppendCallback(() =>
            {
                Anim_SkillDetermined effect = PoolManager.GetItem<Anim_SkillDetermined>();

                effect.transform.position = skillPiece.skillImg.transform.position;
                effect.SetRotation(skillPiece.skillImg.transform.eulerAngles);
                effect.SetScale(1.1f);

                effect.Play();
            })
            //.Join(skillPiece.transform.DOScale(Vector3.one, 0.5f))
            .AppendInterval(0.3f)
            .Append(skillPiece.GetComponent<Image>().DOFade(0, 0.3f))
            .Join(skillPiece.skillImg.DOFade(0, 0.3f))
            .OnComplete(() =>
            { //print("이펙트끝남");
                skillPiece.gameObject.SetActive(false);
                onEndEffect();
            });

        ShowPanel(true);
    }

    public void ShowCasting(PieceInfo info, Action onEndEffect)
    {
        cardBG.sprite = info.cardBG;
        cardNameText.text = info.PieceName;
        cardDesText.text = info.PieceDes;

        pieceMoveSequence.Kill();

        onEndEffect();

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

    public void ShowPanel(bool enable, bool skip = false, Action endEvent = null)
    {
        showSequence.Kill();
        if (!skip)
        {
            showSequence = DOTween.Sequence().Append(cvsGroup.DOFade(enable ? 1 : 0, enable ? 0.2f : 0.5f).OnComplete(() =>
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
