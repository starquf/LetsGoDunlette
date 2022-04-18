using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainUIHandler : MonoBehaviour
{
    public RectTransform titleImage;
    public RectTransform touchText;

    public CanvasGroup chooseDeckPanel;

    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);

        chooseDeckPanel.alpha = 0;
        chooseDeckPanel.gameObject.SetActive(false);
    }

    private void OnClickButton()
    {
        titleImage.DOAnchorPosY(1300, 1.5f);
        touchText.DOAnchorPosY(-1300, 1.5f);

        chooseDeckPanel.gameObject.SetActive(true);
        chooseDeckPanel.DOFade(1f, 1f);
    }
}
