using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterHandler : MonoBehaviour
{
    public ToBeContinueHandler tbHandler;
    public ShopEncounterUIHandler shopEncounterUIHandler;
    public RandomEncounterUIHandler randomEncounterUIHandler;
    public RestEncounterUIHandler restEncounterUIHandler;

    private BattleHandler bh = null;

    private void Awake()
    {
        GameManager.Instance.OnEndEncounter += EndEncounter;
    }

    private void Start()
    {
        bh = GameManager.Instance.battleHandler;
        bh.GetComponent<BattleScrollHandler>().ShowScrollUI(open: false,skip: true);
        GameManager.Instance.mapHandler.OpenMapPanel(true, true);
        //StartEncounter(mapNode.MONSTER);
    }

    // 인카운터 시작할 떄 호출
    public void StartEncounter(mapNode type)
    {
        GameManager.Instance.mapHandler.OpenMapPanel(false);
        CheckEncounter(type);
    }

    private void CheckEncounter(mapNode type)
    {
        switch (type)
        {
            case mapNode.NONE:
                print("???");
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                tbHandler.StartEvent();
                break;
            case mapNode.START:
                break;
            case mapNode.BOSS:
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                bh.StartBattle(isBoss : true);
                break;
            case mapNode.EMONSTER:
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                bh.StartBattle(isElite: true);
                break;
            case mapNode.MONSTER:
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                shopEncounterUIHandler.StartEvent();
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
                break;
        }
    }

    private void EndEncounter()
    {
        print("인카운터 끝남");
        bh.GetComponent<BattleScrollHandler>().ShowScrollUI(open:false);
        GameManager.Instance.mapHandler.OpenMapPanel(true);
    }
}
