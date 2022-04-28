using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EncounterHandler : MonoBehaviour
{
    public Image swapBlackPanel;
    public ShopEncounterUIHandler shopEncounterUIHandler;
    public RandomEncounterUIHandler randomEncounterUIHandler;
    public RestEncounterUIHandler restEncounterUIHandler;

    public CanvasGroup fadeBGCvs;

    private BattleHandler bh = null;

    private bool isEncounterPlaying = false;

    private void Awake()
    {
        GameManager.Instance.encounterHandler = this;
        isEncounterPlaying = false;
    }

    private void Start()
    {
        GameManager.Instance.OnResetGame += () => isEncounterPlaying = false;;
        GameManager.Instance.OnEndEncounter += EndEncounter;
        bh = GameManager.Instance.battleHandler;
        bh.GetComponent<BattleScrollHandler>().ShowScrollUI(open: false,skip: true);
        GameManager.Instance.goldUIHandler.ShowGoldUI(false, true);

        StartCoroutine(LateStart());
        //StartEncounter(mapNode.MONSTER);
    }

    private IEnumerator LateStart()
    {
        yield return null;

        GameManager.Instance.mapManager.OpenMap(true, first: true);
    }

    // ��ī���� ������ �� ȣ��
    public void StartEncounter(mapNode type)
    {
        if (isEncounterPlaying) return;
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
                Debug.LogError("�߸��� ��ī���Ͱ� �����");
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
                break;
            case mapNode.SHOP:
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                shopEncounterUIHandler.StartEvent();
                break;
            case mapNode.REST:
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                restEncounterUIHandler.StartEvent();
                break;
            case mapNode.RandomEncounter:
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                randomEncounterUIHandler.StartEvent();
                GameManager.Instance.bottomUIHandler.ShowBottomPanel(false);
                break;
        }
    }

    private void EndEncounter()
    {
        if (!isEncounterPlaying) return;
        isEncounterPlaying = false;
        GameManager.Instance.curEncounter = mapNode.NONE;
        bh.GetComponent<BattleScrollHandler>().ShowScrollUI(open:false);
        GameManager.Instance.goldUIHandler.ShowGoldUI(false);
        GameManager.Instance.bottomUIHandler.ShowBottomPanel(true);

        Sequence mapChangeSeq = DOTween.Sequence()
            .Append(fadeBGCvs.DOFade(1f, 0.5f).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
                GameManager.Instance.mapManager.OpenMap(true);
            })
            .Append(fadeBGCvs.DOFade(0f, 0.7f).SetEase(Ease.Linear))
            .SetUpdate(true);
    }
}
