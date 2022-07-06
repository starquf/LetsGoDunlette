using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : MainUIPanel
{
    public Button tapBtn;
    public Text touchText;

    public MainUIPanel lobbyPanel;

    protected void Start()
    {
        tapBtn.onClick.AddListener(() =>
        {
            mainUIHandler.ChangePanel(this, lobbyPanel);
        });

        DOTween.Sequence()
            .Append(touchText.transform.DOScale(Vector3.one * 1.1f, 1f).SetEase(Ease.Linear))
            .Append(touchText.transform.DOScale(Vector3.one, 1f).SetEase(Ease.Linear))
            .SetLoops(-1, LoopType.Restart);
    }
}
