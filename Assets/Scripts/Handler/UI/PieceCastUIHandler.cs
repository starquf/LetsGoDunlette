using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PieceCastUIHandler : MonoBehaviour
{
    public Transform parent;

    [Header("ī?? UI")]
    public Image cardBG;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardDesText;
    public Transform skillIconTrans;
    public Image strokeImg;
    public Image targetBGImg;
    public Image targetIcon;
    public GradeInfoHandler gradeHandler;

    public Button closeBtn;

    private CanvasGroup cvsGroup;
    private Sequence showSequence;
    private Sequence pieceMoveSequence;

    [Header("??????")]
    public List<Color> colors = new List<Color>();
    public Dictionary<ElementalType, Color> colorDic;

    private List<SkillDesIcon> desIcons = new List<SkillDesIcon>();

    private Coroutine timeCor;
    private readonly WaitForSeconds fiveSecWait = new WaitForSeconds(5f);

    private Sequence skipSeq;
    public SkipUIPanelHandler skipUI;

    private BattleHandler battleHandler;

    public IconInfoHandler iconInfoHandler;

    private void Awake()
    {
        cvsGroup = GetComponent<CanvasGroup>();

        skillIconTrans.GetComponentsInChildren(desIcons);

        ShowPanel(false, true);

        colorDic = new Dictionary<ElementalType, Color>();
        for (int i = 0; i < colors.Count; i++)
        {
            colorDic.Add((ElementalType)i, colors[i]);
        }

    }

    private void Start()
    {
        battleHandler = GameManager.Instance.battleHandler;
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

        if (cardDesText.text.Equals(""))
        {
            cardDesText.gameObject.SetActive(false);
        }
        else
        {
            cardDesText.gameObject.SetActive(true);
        }

        List<DesIconInfo> desInfos = skillPiece.GetDesIconInfo();
        ShowDesIcon(desInfos, skillPiece);

        InventoryHandler inven = GameManager.Instance.inventoryHandler;

        strokeImg.sprite = inven.pieceBGStrokeSprDic[skillPiece.currentType];
        targetBGImg.sprite = inven.targetBGSprDic[skillPiece.currentType];
        targetIcon.sprite = inven.targetIconSprDic[skillPiece.skillRange];
        gradeHandler.SetGrade(skillPiece.skillGrade);

        pieceMoveSequence.Kill();

        skillPiece.gameObject.SetActive(true);
        skillPiece.transform.SetParent(parent);

        iconInfoHandler.InitInfo(skillPiece, skillPiece.usedIcons);

        pieceMoveSequence = DOTween.Sequence()
            .Append(skillPiece.transform.DOMove(parent.position, 0.25f))
            .Join(skillPiece.transform.DORotate(Quaternion.Euler(0, 0, 30).eulerAngles, 0.3f))
            //.Join(skillPiece.transform.DOScale(Vector3.one, 0.5f))
            //.AppendInterval(0.1f)
            .Append(skillPiece.GetComponent<Image>().DOFade(0, 0.4f))
            .Join(skillPiece.skillIconImg.DOFade(0, 0.4f))
            .AppendCallback(() =>
            {
                skillPiece.gameObject.SetActive(false);
            })
            .InsertCallback(0.2f, () =>
            { //print("????Ʈ????");
                onEndEffect += () => { skillPiece.gameObject.SetActive(false); };

                timeCor = StartCoroutine(CastWait(onEndEffect));

                ShowSkipText();

                SetCloseBtn(() =>
                {
                    if (timeCor != null)
                    {
                        StopCoroutine(timeCor);
                    }

                    onEndEffect();
                });
            });

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)cardDesText.transform.parent);

        ShowPanel(true);
    }

    private void ShowDesIcon(List<DesIconInfo> desInfos, SkillPiece skillPiece)
    {
        int count = 0;
        skillIconTrans.gameObject.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            DesIconType type = desInfos[i].iconType;

            if (type.Equals(DesIconType.None))
            {
                count++;
                desIcons[i].gameObject.SetActive(false);
                continue;
            }
            else
            {
                desIcons[i].gameObject.SetActive(true);
            }

            Sprite icon = battleHandler.battleUtil.GetDesIcon(skillPiece, type);

            desIcons[i].SetIcon(icon, desInfos[i].value);
        }

        if (count >= 3)
        {
            skillIconTrans.gameObject.SetActive(false);
        }
    }

    public void ShowCasting(PieceInfo info, Action onEndEffect)
    {
        cardBG.sprite = info.cardBG;
        cardNameText.text = info.PieceName;
        cardDesText.text = info.PieceDes;

        for (int i = 0; i < 3; i++)
        {
            desIcons[i].gameObject.SetActive(false);
        }

        if (cardDesText.text.Equals(""))
        {
            cardDesText.gameObject.SetActive(false);
        }
        else
        {
            cardDesText.gameObject.SetActive(true);
        }

        pieceMoveSequence.Kill();

        timeCor = StartCoroutine(CastWait(onEndEffect));

        ShowSkipText();

        SetCloseBtn(() =>
        {
            if (timeCor != null)
            {
                StopCoroutine(timeCor);
            }

            onEndEffect();
        });

        ShowPanel(true);
    }

    public void ShowCasting(PieceInfo info, List<DesIconInfo> desInfos, SkillPiece skillPiece, Action onEndEffect)
    {
        cardBG.sprite = info.cardBG;
        cardNameText.text = info.PieceName;
        cardDesText.text = info.PieceDes;

        if (cardDesText.text.Equals(""))
        {
            cardDesText.gameObject.SetActive(false);
        }
        else
        {
            cardDesText.gameObject.SetActive(true);
        }

        ShowDesIcon(desInfos, skillPiece);
        iconInfoHandler.InitInfo(skillPiece, skillPiece.usedIcons);
        InventoryHandler inven = GameManager.Instance.inventoryHandler;
        strokeImg.sprite = inven.pieceBGStrokeSprDic[skillPiece.currentType];
        targetBGImg.sprite = inven.targetBGSprDic[skillPiece.currentType];
        targetIcon.sprite = inven.targetIconSprDic[skillPiece.skillRange];
        gradeHandler.SetGrade(skillPiece.skillGrade);

        pieceMoveSequence.Kill();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)cardDesText.transform.parent);

        timeCor = StartCoroutine(CastWait(onEndEffect));

        ShowSkipText();

        SetCloseBtn(() =>
        {
            if (timeCor != null)
            {
                StopCoroutine(timeCor);
            }

            onEndEffect();
        });

        ShowPanel(true);
    }

    private void ShowSkipText()
    {
        skipSeq = DOTween.Sequence()
                    .AppendInterval(0.5f)
                    .AppendCallback(() =>
                    {
                        skipUI.ShowSkipUI();
                    });
    }

    public void EndCast(SkillPiece skillPiece)
    {
        pieceMoveSequence.Kill();
        skillPiece.gameObject.SetActive(false);

        ShowPanel(false, false, () =>
        {
            if (skillPiece != null)
            {
                skillPiece.GetComponent<Image>().color = Color.white;
                skillPiece.skillIconImg.color = Color.white;
            }
        });


    }

    private IEnumerator CastWait(Action onEndEffect)
    {
        yield return fiveSecWait;

        closeBtn.onClick.RemoveAllListeners();
        onEndEffect?.Invoke();
    }

    public void SetCloseBtn(Action action)
    {
        closeBtn.onClick.AddListener(() =>
        {
            closeBtn.onClick.RemoveAllListeners();
            action.Invoke();
        });
    }

    public void ShowPanel(bool enable, bool skip = false, Action endEvent = null)
    {
        showSequence.Kill();
        if (!skip)
        {
            skipSeq.Kill();
            skipUI.SetPanel(false);

            showSequence = DOTween.Sequence().Append(cvsGroup.DOFade(enable ? 1 : 0, enable ? 0.2f : 0.3f)
            .OnComplete(() =>
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

            skipSeq.Kill();
            skipUI.SetPanel(false);

            endEvent?.Invoke();
        }
    }
}
