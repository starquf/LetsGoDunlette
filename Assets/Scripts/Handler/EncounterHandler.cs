using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterHandler : MonoBehaviour
{
    public Image swapBlackPanel;
    public ShopEncounterUIHandler shopEncounterUIHandler;
    public RandomEncounterUIHandler randomEncounterUIHandler;
    public RestEncounterUIHandler restEncounterUIHandler;

    public CanvasGroup fadeBGCvs;

    private BattleHandler bh = null;

    private bool isEncounterPlaying = false;

    [Header("아래 UI들")]
    public List<BottomUIElement> bottomUIs = new List<BottomUIElement>();

    private void Awake()
    {
        GameManager.Instance.encounterHandler = this;
        isEncounterPlaying = false;
    }

    private void Start()
    {
        GameManager.Instance.OnResetGame += () => isEncounterPlaying = false; ;
        GameManager.Instance.OnEndEncounter += EndEncounter;
        bh = GameManager.Instance.battleHandler;
        //bh.GetComponent<BattleScrollHandler>().ShowScrollUI(open: false,skip: true);
        //GameManager.Instance.goldUIHandler.ShowGoldUI(false, true);

        StartCoroutine(LateStart());
        //StartEncounter(mapNode.MONSTER);
    }

    private IEnumerator LateStart()
    {
        yield return null;

        GameManager.Instance.mapManager.OpenMap(true, first: true);
    }

    // 인카운터 시작할 떄 호출
    public void StartEncounter(mapNode type)
    {
        if (isEncounterPlaying)
        {
            return;
        }

        for (int i = 0; i < bottomUIs.Count; i++)
        {
            bottomUIs[i].ClosePanel();
            bottomUIs[i].canControl = false;
        }

        ShowBlackPanel(true);
        isEncounterPlaying = true;
        GameManager.Instance.curEncounter = type;

        Sequence mapChangeSeq = DOTween.Sequence()
            .AppendInterval(0.37f)
            .Append(fadeBGCvs.DOFade(1f, 0.5f).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
                GameManager.Instance.mapManager.OpenMap(false, 0.1f);
                CheckEncounter(type);
            })
            .Append(fadeBGCvs.DOFade(0f, 0.7f).SetEase(Ease.Linear))
            .SetUpdate(true);
    }

    public void ShowBlackPanel(bool enable)
    {
        swapBlackPanel.color = enable ? Color.black : Color.clear;
    }

    private void CheckEncounter(mapNode type)
    {
        StartCoroutine(LateEncounter(type));
    }

    private IEnumerator LateEncounter(mapNode type)
    {
        yield return null;

        switch (type)
        {
            case mapNode.NONE:
                ///print("???");
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                //tbHandler.StartEvent();
                Debug.LogError("잘못된 인카운터가 실행됨");
                break;
            case mapNode.START:
                break;
            case mapNode.BOSS:
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                bh.StartBattle(isBoss: true);
                break;
            case mapNode.EMONSTER:
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                bh.StartBattle(isElite: true);
                break;
            case mapNode.MONSTER:
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                bh.StartBattle();
                //randomEncounterUIHandler.StartEvent();
                //GameManager.Instance.bottomUIHandler.ShowBottomPanel(false);
                break;
            case mapNode.SHOP:
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                shopEncounterUIHandler.StartEvent();
                break;
            case mapNode.REST:
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                restEncounterUIHandler.StartEvent();
                break;
            case mapNode.RANDOMENCOUNTER:
                //GameManager.Instance.mapHandler.OpenMapPanel(false);

                int randIdx = Random.Range(0, 100);
                if (randIdx < 15)//몹
                {
                    GameManager.Instance.curEncounter = mapNode.MONSTER;
                    bh.StartBattle();
                }
                else if (randIdx < 17)// 엘몹
                {
                    GameManager.Instance.curEncounter = mapNode.EMONSTER;
                    bh.StartBattle(isElite: true);
                }
                else if (randIdx < 97) // 인카운터
                {
                    randomEncounterUIHandler.StartEvent();
                    GameManager.Instance.bottomUIHandler.ShowBottomPanel(false);
                }
                else if (randIdx < 99) // 상점
                {
                    GameManager.Instance.curEncounter = mapNode.SHOP;
                    shopEncounterUIHandler.StartEvent();
                }
                else // 휴식
                {
                    GameManager.Instance.curEncounter = mapNode.REST;
                    restEncounterUIHandler.StartEvent();
                }
                break;
        }


        for (int i = 0; i < bottomUIs.Count; i++)
        {
            bottomUIs[i].canControl = true;
        }
    }

    private void EndEncounter()
    {
        if (!isEncounterPlaying)
        {
            return;
        }

        isEncounterPlaying = false;

        GameManager gm = GameManager.Instance;

        gm.curEncounter = mapNode.NONE;
        gm.bottomUIHandler.ShowBottomPanel(true);

        bh.SetCanvas(false);

        if (gm.battleHandler.isBoss)
        {
            Sequence nextStageSeq = DOTween.Sequence()
                .Append(fadeBGCvs.DOFade(1f, 0.5f).SetEase(Ease.Linear))
                .AppendCallback(() =>
                {
                    bh.battleInfoHandler.ResetWeakCounter();
                    gm.NextStage();
                })
                .AppendInterval(2f)
                .AppendCallback(() =>
                {
                    GameManager.Instance.mapManager.OpenMap(true, first: true);
                })
                .Append(fadeBGCvs.DOFade(0f, 0.7f).SetEase(Ease.Linear))
                .SetUpdate(true);
        }
        else
        {
            Sequence mapChangeSeq = DOTween.Sequence()
                .Append(fadeBGCvs.DOFade(1f, 0.5f).SetEase(Ease.Linear))
                .AppendCallback(() =>
                {
                    gm.mapManager.OpenMap(true);
                })
                .Append(fadeBGCvs.DOFade(0f, 0.7f).SetEase(Ease.Linear))
                .SetUpdate(true);
        }
    }
}
