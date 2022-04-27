using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleFieldHandler : MonoBehaviour
{
    public Transform fieldHandlersParent;

    private Dictionary<ElementalType, FieldHandler> fieldDic;

    public ElementalType FieldType
    {
        get { return nowFieldType; }
    }
    //필드속성을 저장함 기본은 무속성
    private ElementalType nowFieldType = ElementalType.None;

    private int currentTurn = 0;

    public void Start()
    {
        nowFieldType = ElementalType.None;

        fieldDic = new Dictionary<ElementalType, FieldHandler>();
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

    private void ChangeField(ElementalType type)
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

    public bool CheckFieldType(ElementalType type)
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
            SetFieldType(ElementalType.None);
        }
    }

    public void IncreaseTurn(int turn)
    {
        if (nowFieldType != ElementalType.None)
        {
            currentTurn += turn;
        }
    }

    public void SetFieldType(ElementalType type)
    {
        if (type == ElementalType.Monster)
            return;

        if (type.Equals(ElementalType.None))
        {
            if (!nowFieldType.Equals(ElementalType.None))
            {
                fieldDic[nowFieldType].DisableField();
                nowFieldType = type;

                currentTurn = 0;
            }

            return;
        }

        ChangeField(type);

        nowFieldType = type;
    }
}
