using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class StageRandomEncounter
{
    public List<int> RandomEncounterIdx;
}

public class RandomEncounterUIHandler : MonoBehaviour
{
    private EncounterInfoHandler encounterInfoHandler;

    [Header("인카운터 데이터들")]
    public List<RandomEncounter> randomEncounterList;
    public List<StageRandomEncounter> stage;

    private CanvasGroup mainPanel;
    public CanvasGroup enStartPanel;
    public CanvasGroup enEndPanel;
    public CanvasGroup imgButtonRowsCvs;

    public Image encounterImg;
    public Text encounterTitleTxt, encounterTxt, encounterResultTxt;
    public List<Text> encounterChoiceTxtList;
    public Button exitBtn;

    private RandomEncounter randomEncounter;
    private BattleScrollHandler battleScrollHandler;

    private int encounterIdx;
    private List<int> cantEncounterIdxList;

    private void Awake()
    {
        encounterInfoHandler = GetComponent<EncounterInfoHandler>();
        mainPanel = GetComponent<CanvasGroup>();
        cantEncounterIdxList = new List<int>();
    }

    private void Start()
    {
        battleScrollHandler = GameManager.Instance.battleHandler.GetComponent<BattleScrollHandler>();
        for (int i = 0; i < randomEncounterList.Count; i++)
        {
            randomEncounterList[i].encounterInfoHandler = this.encounterInfoHandler;
            randomEncounterList[i].OnExitEncounter = EndEvent;
            randomEncounterList[i].ShowEndEncounter = ShowEndEncounter;
        }
        exitBtn.onClick.AddListener(OnExitBtnClick);
        encounterIdx = -1;
    }

    public void SetRandomEncounter(int idx)
    {
        encounterIdx = idx;
    }

    public bool CanStartEncounter(int idx)
    {
        for (int i = 0; i < cantEncounterIdxList.Count; i++)
        {
            if( cantEncounterIdxList[i] == idx)
            {
                return false;
            }
        }
        if (idx < 0)
        {
            return false;
        }
        else if (idx == 8 || idx == 10) // 스크롤 없을시 발동 x
        {
            if (!battleScrollHandler.HasScroll())
            {
                return false;
            }
        }
        else if(idx == 13) //힐 스크롤 없을 시 발동x
        {
            for (int i = 0; i < battleScrollHandler.slots.Count; i++)
            {
                ScrollSlot scrollSlot = battleScrollHandler.slots[i];
                if (scrollSlot.scroll != null)
                {
                    if (scrollSlot.scroll.scrollType == ScrollType.Heal)
                    {
                        cantEncounterIdxList.Add(13);
                        return true;
                    }
                }
            }
            return false;
        }
        else if(idx == 16)//전기 스킬 없을시 발동 x
        {
            Inventory inven = GameManager.Instance.inventoryHandler.GetPlayerInventory();

            for (int i = 0; i < inven.skills.Count; i++)
            {
                if(inven.skills[i].currentType == ElementalType.Electric)
                {
                    return true;
                }
            }

            return false;
        }
        return true;
    }

    public void InitEncounter()
    {
        if(encounterIdx < 0)
        {
            int randIdx = -1;
            //randIdx = 16;
            while (!CanStartEncounter(randIdx))
            {
                randIdx = stage[GameManager.Instance.StageIdx].RandomEncounterIdx[Random.Range(0, stage[GameManager.Instance.StageIdx].RandomEncounterIdx.Count)];
            }
            randomEncounter = randomEncounterList[randIdx];
        }
        else
        {
            randomEncounter = randomEncounterList[encounterIdx];
            encounterIdx = -1;
        }

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

        randomEncounter.Init();
    }

    public void InteratableButton(bool enable)
    {
        for (int i = 0; i < 3; i++)
        {
            encounterChoiceTxtList[i].transform.parent.GetComponent<Button>().interactable = enable;
        }
    }

    public void StartEvent()
    {
        InitEncounter();
        ShowPanel(true);
        InteratableButton(true);
    }

    #region OnButtonClick

    private void OnChoiceBtnClick(int choiceIdx)
    {
        randomEncounter.ResultSet(choiceIdx);
        InteratableButton(false);
    }


    private void OnExitBtnClick()
    {
        randomEncounter.Result();
    }

    #endregion

    private void ShowEndEncounter()
    {
        encounterImg.DOColor(Color.black, 0.5f);
        encounterTxt.DOFade(0, 0.5f);
        ShowPanel(false, enStartPanel, 0.3f, () =>
        {
            encounterTxt.text = randomEncounter.showText;
            encounterResultTxt.text = randomEncounter.en_End_Result;
            encounterImg.sprite = randomEncounter.showImg;
            encounterImg.DOColor(Color.white, 0.3f).SetEase(Ease.InQuad);
            encounterTxt.DOFade(1, 0.3f).SetEase(Ease.InQuad);
            ShowPanel(true, enEndPanel);
        });
    }

    private void EndEvent(bool openMap = true)
    {
        ShowPanel(false, null, 0.5f, () =>
        {
            for (int i = 0; i < 3; i++)
            {
                int a = i;
                encounterChoiceTxtList[a].transform.parent.GetComponent<Button>().onClick.RemoveAllListeners();
            }
            ShowPanelSkip(true, enStartPanel);
            ShowPanelSkip(false, enEndPanel);
            ShowPanelSkip(false, imgButtonRowsCvs);
            exitBtn.gameObject.SetActive(true);
        });

        if (openMap)
        {
            GameManager.Instance.EndEncounter();
        }
    }

    public void ShowPanel(bool enable, CanvasGroup cvsGroup = null, float time = 0.5f, Action onComplecteEvent = null)
    {
        if(cvsGroup == null)
        {
            cvsGroup = mainPanel;
        }
        if(!enable)
        {
            cvsGroup.blocksRaycasts = enable;
            cvsGroup.interactable = enable;
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
