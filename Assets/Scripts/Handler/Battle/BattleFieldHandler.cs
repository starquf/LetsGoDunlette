using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BattleFieldHandler : MonoBehaviour
{
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
    }

    public void SetFieldType(PatternType type)
    {
        nowFieldType = type;
    }
}
