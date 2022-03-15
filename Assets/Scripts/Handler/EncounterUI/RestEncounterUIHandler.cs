using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RestEncounterUIHandler : MonoBehaviour
{
    private CanvasGroup mainPanel;
    public CanvasGroup restSelectPanel;
    public CanvasGroup restResultPanel;

    public Button RestBtn, UpgradePieceBtn, EatMinsooBtn, ExitBtn;

    private void Awake()
    {
        mainPanel = GetComponent<CanvasGroup>();
        RestBtn.onClick.AddListener(OnRestBtnClick);
        UpgradePieceBtn.onClick.AddListener(OnUpgradeBtnClick);
        EatMinsooBtn.onClick.AddListener(OnEatMinsooBtnClick);
        ExitBtn.onClick.AddListener(OnExitBtnClick);
    }

    public void StartEvent()
    {
        ShowPanel(true);
    }

    #region OnButtonClick

    private void OnRestBtnClick()
    {
        ShowPanel(false, restSelectPanel, 0.3f, () =>
        {
            ShowPanel(true, restResultPanel, 0.5f, ()=>
            {
                GameManager.Instance.GetPlayer().Heal(10);
            });
        });
    }

    private void OnUpgradeBtnClick()
    {
        print("���� �����ȵ� ��߸�");
        EndEvent();
    }


    private void OnEatMinsooBtnClick()
    {
        print("���� �����ȵ� ��߸�");
        EndEvent();
    }

    private void OnExitBtnClick()
    {

        EndEvent();
    }

    #endregion

    private void EndEvent()
    {
        ShowPanel(false, null, 0.5f, () =>
        {
            ShowPanelSkip(true, restSelectPanel);
            ShowPanelSkip(false, restResultPanel);
        });

        GameManager.Instance.EndEncounter();
    }

    public void ShowPanel(bool enable, CanvasGroup cvsGroup = null, float time = 0.5f, Action onComplecteEvent = null)
    {
        if(cvsGroup == null)
        {
            cvsGroup = mainPanel;
        }
        cvsGroup.DOFade(enable ? 1 : 0, time)
            .OnComplete(() => {
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