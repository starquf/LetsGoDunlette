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
        StartEncounter(EncounterType.Battle);
    }

    // 인카운터 시작할 떄 호출
    public void StartEncounter(EncounterType type)
    {
        CheckEncounter(type);
    }

    private void CheckEncounter(EncounterType type)
    {
        switch (type)
        {
            case EncounterType.Battle:

                print("전투 발생!!");
                GameManager.Instance.battleHandler.StartBattle();

                break;

            case EncounterType.RandomEvent:

                print("랜덤 이벤트 발생!!");

                break;

            case EncounterType.Rest:

                print("휴식 발생!!");

                break;

            case EncounterType.BossBattle:

                print("보스 전투 발생!!");

                break;
        }
    }

    private void EndEncounter()
    {
        print("인카운터 끝남");
    }
}
