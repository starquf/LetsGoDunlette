using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterHandler : MonoBehaviour
{
    public ToBeContinueHandler tbHandler;
    public ShopEncounterUIHandler shopEncounterUIHandler;
    public RandomEncounterUIHandler randomEncounterUIHandler;
    public RestEncounterUIHandler restEncounterUIHandler;

    private void Awake()
    {
        GameManager.Instance.OnEndEncounter += EndEncounter;
    }

    private void Start()
    {
        GameManager.Instance.mapHandler.OpenMapPanel(true, true);
        //StartEncounter(mapNode.MONSTER);
    }

    // ��ī���� ������ �� ȣ��
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
                print("����");
                break;
            case mapNode.BOSS:
                print("����");
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                GameManager.Instance.battleHandler.StartBattle(isBoss : true);
                break;
            case mapNode.EMONSTER:
                print("����Ʈ ��");
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                tbHandler.StartEvent();
                break;
            case mapNode.MONSTER:
                print("��");
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                GameManager.Instance.battleHandler.StartBattle();
                break;
            case mapNode.SHOP:
                print("����");
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                shopEncounterUIHandler.StartEvent();
                break;
            case mapNode.REST:
                print("�޽�");
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                restEncounterUIHandler.StartEvent();
                break;
            case mapNode.RandomEncounter:
                print("����");
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                randomEncounterUIHandler.StartEvent();
                break;
        }
    }

    private void EndEncounter()
    {
        print("��ī���� ����");
        GameManager.Instance.mapHandler.OpenMapPanel(true);
    }
}
