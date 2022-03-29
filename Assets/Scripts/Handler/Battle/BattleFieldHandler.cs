using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BattleFieldHandler : MonoBehaviour
{
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
    }

    public void SetFieldType(PatternType type)
    {
        switch (type)
        {
            case PatternType.None:
                break;
            case PatternType.Clover:
                break;
            case PatternType.Diamonds:
                break;
            case PatternType.Heart:
                break;
            case PatternType.Spade:
                break;
            case PatternType.Monster:
                break;
            default:
                break;
        }
        nowFieldType = type;
    }
}
