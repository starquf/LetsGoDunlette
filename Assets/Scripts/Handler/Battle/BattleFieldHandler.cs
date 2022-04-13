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
    //�ʵ�Ӽ��� ������ �⺻�� ���Ӽ�
    private PatternType nowFieldType = PatternType.None;

    public void Awake()
    {
        GameManager.Instance.battleFieldHandler = this;

    }

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

    public void ChangeField(PatternType type)
    {
        foreach (FieldHandler fieldHandler in fieldDic.Values)
        {
            fieldHandler.DisableField();
        }

        fieldDic[type].EnableField();
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
            Debug.LogError("���� Ÿ���� �ʵ�� �����ϴ�");
        nowFieldType = type;
        ChangeField(nowFieldType);
    }
}
