using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RandomEncounterUIHandler : MonoBehaviour
{
    [Header("인카운터 데이터들")]
    public List<RandomEncounter> randomEncounterList;

    private CanvasGroup mainPanel;
    public CanvasGroup enStartPanel;
    public CanvasGroup enEndPanel;

    public Image encounterImg, whiteImg;
    public Text encounterTitleTxt, encounterTxt, encounterResultTxt;
    public List<Text> encounterChoiceTxtList;
    public Button ExitBtn;

    private RandomEncounter randomEncounter;

    private void Awake()
    {
        mainPanel = GetComponent<CanvasGroup>();
        ExitBtn.onClick.AddListener(OnExitBtnClick);
    }

    public void InitEncounter()
    {
        randomEncounter = randomEncounterList[Random.Range(0, randomEncounterList.Count)];

        encounterTitleTxt.text = randomEncounter.en_Name;
        encounterTxt.text = randomEncounter.en_Start_Text;
        encounterImg.sprite = randomEncounter.en_Start_Image;
        
        for (int i = 0; i < 3; i++)
        {
            if(randomEncounter.en_Choice_Count <= i)
            {
                encounterChoiceTxtList[i].transform.parent.gameObject.SetActive(false);
            }
            else
            {
                encounterChoiceTxtList[i].transform.parent.gameObject.SetActive(true);
                encounterChoiceTxtList[i].text = randomEncounter.en_ChoiceList[i];

                int idx = i;
                encounterChoiceTxtList[i].transform.parent.GetComponent<Button>().onClick.AddListener(()=>
                {
                    OnChoiceBtnClick(idx);
                });
            }
        }
    }

    public void StartEvent()
    {
        InitEncounter();
        ShowPanel(true);
    }

    #region OnButtonClick

    private void OnChoiceBtnClick(int choiceIdx)
    {
        randomEncounter.ResultSet(choiceIdx);

        whiteImg.DOFade(1, 0.3f);
        ShowPanel(false, enStartPanel, 0.3f, ()=>
        {
            encounterTxt.text = randomEncounter.en_End_TextList[choiceIdx];
            encounterResultTxt.text = randomEncounter.en_End_Result;
            encounterImg.sprite = randomEncounter.showImg;
            whiteImg.DOFade(0, 0.3f).SetEase(Ease.InQuad);
            ShowPanel(true, enEndPanel);
        });
        //randomEncounter.Result();
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
            ShowPanelSkip(true, enStartPanel);
            ShowPanelSkip(false, enEndPanel);
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
