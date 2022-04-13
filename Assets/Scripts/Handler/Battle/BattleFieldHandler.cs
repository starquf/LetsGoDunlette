using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleFieldHandler : MonoBehaviour
{
    public Transform fieldHandlersParent;

    private Dictionary<PatternType, FieldHandler> fieldDic;

    public PatternType FieldType
    {
        get { return nowFieldType; }
    }
    //필드속성을 저장함 기본은 무속성
    private PatternType nowFieldType = PatternType.None;

    private int currentTurn = 0;

    public void Start()
    {
        nowFieldType = PatternType.None;

        fieldDic = new Dictionary<PatternType, FieldHandler>();
        for (int i = 0; i < fieldHandlersParent.childCount; i++)
        {
            FieldHandler field = fieldHandlersParent.GetChild(i).GetComponent<FieldHandler>();

            fieldDic.Add(field.fieldType, field);
        }

        foreach(FieldHandler fieldHandler in fieldDic.Values)
        {
            fieldHandler.DisableField(true);
        }
    }

    private void ChangeField(PatternType type)
    {
        foreach (FieldHandler fieldHandler in fieldDic.Values)
        {
            fieldHandler.DisableField();
        }

        if (fieldDic.TryGetValue(type, out FieldHandler field))
        {
            field.EnableField();

            currentTurn = 5;
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

    public void DecreaseTurn()
    {
        currentTurn--;

        if (currentTurn <= 0)
        {
            currentTurn = 0;
            SetFieldType(PatternType.None);
        }
    }

    public void IncreaseTurn(int turn)
    {
        if (nowFieldType != PatternType.None)
        {
            currentTurn += turn;
        }
    }

    public void SetFieldType(PatternType type)
    {
        if (type == PatternType.Monster)
            return;

        if (type.Equals(PatternType.None))
        {
            if (!nowFieldType.Equals(PatternType.None))
            {
                fieldDic[nowFieldType].DisableField();
                currentTurn = 0;
            }

            return;
        }

        ChangeField(type);

        nowFieldType = type;
    }
}
