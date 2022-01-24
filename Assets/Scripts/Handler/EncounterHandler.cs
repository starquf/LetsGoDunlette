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
                break;
            case mapNode.START:
                break;
            case mapNode.BOSS:
                break;
            case mapNode.MONSTER:
                //GameManager.Instance.mapHandler.OpenMapPanel(false);
                GameManager.Instance.battleHandler.StartBattle();
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
        print("인카운터 끝남");
        GameManager.Instance.mapHandler.OpenMapPanel(true);
    }
}
