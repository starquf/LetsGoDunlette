using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class RestEncounterUIHandler : MonoBehaviour
{
    private CanvasGroup mainPanel;
    public CanvasGroup restSelectPanel;
    public CanvasGroup restResultPanel;

    public Button RestBtn, UpgradePieceBtn, ExitBtn;

    private void Awake()
    {
        mainPanel = GetComponent<CanvasGroup>();
        mainPanel.alpha = 0;
        mainPanel.blocksRaycasts = false;
        mainPanel.interactable = false;
    }

    private void Start()
    {
        RestBtn.onClick.AddListener(OnRestBtnClick);
        UpgradePieceBtn.onClick.AddListener(OnUpgradeBtnClick);
        ExitBtn.onClick.AddListener(OnExitBtnClick);
    }

    public void StartEvent()
    {
        ShowPanel(true);
    }

    #region OnButtonClick

    private void OnRestBtnClick()
    {
        RestBtn.interactable = false;
        ShowPanel(false, restSelectPanel, 0.3f, () =>
        {
            ShowPanel(true, restResultPanel, 0.5f, () =>
            {
                PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
                playerHealth.Heal((int)(playerHealth.maxHp * 0.4f));
            });
        });
    }

    private void OnUpgradeBtnClick()
    {
        //print("아직 구현안됨 사발면");
        EndEvent();
    }

    private void OnExitBtnClick()
    {
        ExitBtn.interactable = false;
        EndEvent();
    }

    #endregion

    private void EndEvent()
    {
        ShowPanel(false, null, 0.5f, () =>
        {
            ShowPanelSkip(true, restSelectPanel);
            ShowPanelSkip(false, restResultPanel);
            RestBtn.interactable = true;
            ExitBtn.interactable = true;
        });

        GameManager.Instance.EndEncounter();
    }

    public void ShowPanel(bool enable, CanvasGroup cvsGroup = null, float time = 0.5f, Action onComplecteEvent = null)
    {
        if (cvsGroup == null)
        {
            cvsGroup = mainPanel;
        }
        cvsGroup.DOFade(enable ? 1 : 0, time)
            .OnComplete(() =>
            {
                cvsGroup.blocksRaycasts = enable;
                cvsGroup.interactable = enable;
                onComplecteEvent?.Invoke();
            });
    }
    public void ShowPanelSkip(bool enable, CanvasGroup cvsGroup = null)
    {
        if (cvsGroup == null)
        {
            cvsGroup = mainPanel;
        }
        cvsGroup.alpha = enable ? 1 : 0;
        cvsGroup.blocksRaycasts = enable;
        cvsGroup.interactable = enable;
    }
}
