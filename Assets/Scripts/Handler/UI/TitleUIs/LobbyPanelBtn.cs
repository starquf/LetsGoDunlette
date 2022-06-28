using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LobbyPanelBtn : MonoBehaviour
{
    private Image iconImg;
    private TextMeshProUGUI iconText;

    private Tween textTween;
    private Tween iconTween;

    private readonly Color transColor = new Color(1f, 1f, 1f, 0f);

    private Vector3 iconOrigin;

    private void Awake()
    {
        iconImg = transform.GetChild(0).GetComponent<Image>();
        iconText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        iconOrigin = iconImg.transform.localPosition;

        ShowText(false, isSkip:true);
    }

    public void SetHighlight(bool isHighlight)
    {
        if (isHighlight)
        {
            ShowText(true);
            MoveIcon(true);
        }
        else 
        {
            ShowText(false);
            MoveIcon(false);
        }
    }

    public void ShowText(bool enable, bool isSkip = false)
    {
        textTween.Kill();

        if (isSkip)
        {
            iconText.color = enable ? Color.white : transColor;
            return;
        }

        textTween = iconText.DOFade(enable ? 1f : 0f, 0.35f);
    }

    public void MoveIcon(bool isMove, bool isSkip = false)
    {
        iconTween.Kill();

        if (isSkip)
        {
            iconImg.transform.position = isMove ? iconOrigin + (Vector3.up * 75f) : iconOrigin;
            return;
        }

        iconTween = iconImg.transform.DOLocalMove(isMove ? iconOrigin + (Vector3.up * 75f) : iconOrigin, 0.35f);
    }
}
