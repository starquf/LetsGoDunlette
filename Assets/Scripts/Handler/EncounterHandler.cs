using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterHandler : MonoBehaviour
{
    public ToBeContinueHandler tbHandler;
    public ShopEncounterUIHandler shopEncounterUIHandler;
    public RandomEncounterUIHandler randomEncounterUIHandler;

    private void Awake()
    {
        GameManager.Instance.OnEndEncounter += EndEncounter;
    }

    private void Start()
    {
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
                print("시작");
                break;
            case mapNode.BOSS:
                print("보스");
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                tbHandler.StartEvent();
                break;
            case mapNode.EMONSTER:
                print("엘리트 몹");
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                tbHandler.StartEvent();
                break;
            case mapNode.MONSTER:
                print("적");
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                GameManager.Instance.battleHandler.StartBattle();
                break;
            case mapNode.SHOP:
                print("상점");
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                shopEncounterUIHandler.StartEvent();
                break;
            case mapNode.REST:
                print("휴식");
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                tbHandler.StartEvent();
                break;
            case mapNode.RandomEncounter:
                print("보물");
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                randomEncounterUIHandler.StartEvent();
                break;
        }
    }

    private void EndEncounter()
    {
        print("인카운터 끝남");
        GameManager.Instance.mapHandler.OpenMapPanel(true);
    }
}
