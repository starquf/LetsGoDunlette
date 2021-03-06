using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanelHandler : MonoBehaviour
{
    [SerializeField] private Image gameOverImg;
    private CanvasGroup cvsGroup;

    private Sequence showSequence;

    private void Awake()
    {
        cvsGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        GameManager.Instance.OnResetGame += () =>
        {
            GameManager.Instance.StageIdx = 0;
            GameOverEffect();
        };
    }

    public void GameOverEffect()
    {
        ShowPanel(true, () =>
        {
            DOTween.Sequence().Append(gameOverImg.DOFade(1, 2f))
            .OnComplete(() =>
            {
                //print("?̰ſ? ?ȵ?");
                /*
                GameManager.Instance.mapHandler.OpenMapPanel(true);
                ShowPanel(false, null, true);
                gameOverImg.color = new Color(1f, 1f, 1f, 0f);*/

                GameManager.Instance.StageIdx = 0;
                GameManager.Instance.LoadScene(0);
            });
        });
    }

    private void ShowPanel(bool enable, Action onShowEnd = null, bool skip = false)
    {
        showSequence.Kill();
        if (!skip)
        {
            showSequence = DOTween.Sequence().Append(cvsGroup.DOFade(enable ? 1 : 0, 0.5f).OnComplete(() =>
            {
                cvsGroup.interactable = enable;
                cvsGroup.blocksRaycasts = enable;
                onShowEnd?.Invoke();
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
