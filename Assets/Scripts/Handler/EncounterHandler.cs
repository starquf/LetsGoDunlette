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

    // ��ī���� ������ �� ȣ��
    public void StartEncounter(EncounterType type)
    {
        CheckEncounter(type);
    }

    private void CheckEncounter(EncounterType type)
    {
        switch (type)
        {
            case EncounterType.Battle:

                print("���� �߻�!!");
                GameManager.Instance.battleHandler.StartBattle();

                break;

            case EncounterType.RandomEvent:

                print("���� �̺�Ʈ �߻�!!");

                break;

            case EncounterType.Rest:

                print("�޽� �߻�!!");

                break;

            case EncounterType.BossBattle:

                print("���� ���� �߻�!!");

                break;
        }
    }

    private void EndEncounter()
    {
        print("��ī���� ����");
    }
}
