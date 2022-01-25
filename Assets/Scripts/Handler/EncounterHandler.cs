using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterHandler : MonoBehaviour
{
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
                break;
            case mapNode.START:
                print("����");
                break;
            case mapNode.BOSS:
                print("����");
                break;
            case mapNode.EMONSTER:
                print("����Ʈ ��");
                break;
            case mapNode.MONSTER:
                print("��");
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                GameManager.Instance.battleHandler.StartBattle();
                break;
            case mapNode.SHOP:
                print("����");
                break;
            case mapNode.REST:
                print("�޽�");
                break;
            case mapNode.TREASURE:
                print("����");
                break;
        }
    }

    private void EndEncounter()
    {
        print("��ī���� ����");
        GameManager.Instance.mapHandler.OpenMapPanel(true);
    }
}
