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

    public CanvasGroup topPanel;
    public CanvasGroup bottomPanel;

    private void Start()
    {
        GameManager.Instance.SetResolution();

        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);

        chooseDeckPanel.alpha = 0;
        chooseDeckPanel.gameObject.SetActive(false);

        touchText.GetComponent<Text>().DOFade(1f, 0.5f)
            .From(0f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnClickButton()
    {
        titleImage.DOAnchorPosY(1300, 0.5f)
            .SetEase(Ease.InBack);

        touchText.DOAnchorPosY(-1300, 0.5f)
            .SetDelay(0.1f)
            .SetEase(Ease.InBack);

        chooseDeckPanel.gameObject.SetActive(true);
        chooseDeckPanel.DOFade(1f, 1f);

        DOTween.Sequence()
            .AppendInterval(0.3f)
            .Append(topPanel.DOFade(1f, 0.5f).From(0f))
            .Join(topPanel.transform.DOLocalMoveY(900f, 0.5f).From(true).SetEase(Ease.OutBack, 0.6f))
            .Insert(0.55f, bottomPanel.DOFade(1f, 0.5f).From(0f))
            .Join(bottomPanel.transform.DOLocalMoveY(-900f, 0.5f).From(true).SetEase(Ease.OutBack, 0.6f));
    }
}
