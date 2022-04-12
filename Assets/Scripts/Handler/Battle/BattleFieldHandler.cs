using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleFieldHandler : MonoBehaviour
{
    public Transform fieldHandlersParent;
    public List<FieldHandler> fieldHandlers;

    public PatternType FieldType
    {
        get { return nowFieldType; }
    }
    //필드속성을 저장함 기본은 무속성
    private PatternType nowFieldType = PatternType.None;

    public void Awake()
    {
        GameManager.Instance.battleFieldHandler = this;
    }

    public void Start()
    {
        nowFieldType = PatternType.None;
        fieldHandlers = new List<FieldHandler>();
        fieldHandlers.AddRange(fieldHandlersParent.GetComponentsInChildren<FieldHandler>());
        for (int i = 0; i < fieldHandlers.Count; i++)
        {
            fieldHandlers[i].DisableField(true);
        }
    }

    /*
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ChangeField(PatternType.Clover);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeField(PatternType.Diamonds);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeField(PatternType.Heart);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeField(PatternType.Spade);
        }
    }*/

    public void ChangeField(PatternType type)
    {
        for (int i = 0; i < fieldHandlers.Count; i++)
        {
            fieldHandlers[i].DisableField();
        }
        switch (type)
        {
            case PatternType.None:
                break;
            case PatternType.Clover:
                fieldHandlers.Find(x => x.GetComponent<N_FieldHandler>() != null).EnableField();
                break;
            case PatternType.Diamonds:
                fieldHandlers.Find(x => x.GetComponent<E_FieldHandler>() != null).EnableField();
                break;
            case PatternType.Heart:
                fieldHandlers.Find(x => x.GetComponent<F_FieldHandler>() != null).EnableField();
                break;
            case PatternType.Spade:
                fieldHandlers.Find(x => x.GetComponent<W_FieldHandler>() != null).EnableField();
                break;
            case PatternType.Monster:
                break;
            default:
                break;
        }
    }

    public bool CheckFieldType(PatternType type)
    {
        if(type == FieldType)
        {
            return true;
        }

        return false;
    }

    public void SetFieldType(PatternType type)
    {
        if (type == PatternType.Monster)
            Debug.LogError("몬스터 타입의 필드는 없습니다");
        nowFieldType = type;
        ChangeField(nowFieldType);
    }
}
