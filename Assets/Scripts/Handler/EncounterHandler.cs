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
        //StartEncounter(mapNode.MONSTER);
    }

    // ��ī���� ������ �� ȣ��
    public void StartEncounter(mapNode type)
    {
        CheckEncounter(type);
    }

    private void CheckEncounter(mapNode type)
    {
        switch (type)
        {
            case mapNode.NONE:
                break;
            case mapNode.START:
                break;
            case mapNode.BOSS:
                break;
            case mapNode.MONSTER:
                //GameManager.Instance.battleHandler.StartBattle();
                break;
            case mapNode.SHOP:
                break;
            case mapNode.REST:
                break;
            case mapNode.TREASURE:
                break;
        }
    }

    private void EndEncounter()
    {
        print("��ī���� ����");
    }
}
