using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugFieldPanelHandler : MonoBehaviour, IDebugPanel
{
    private BattleFieldHandler bf;

    [Header("필드 관련들")]
    public Dropdown fieldDropdown;
    public Button fieldSetBtn;

    private Dictionary<PatternType, string> fieldDic;

    public void Init()
    {
        bf = GameManager.Instance.battleFieldHandler;

        fieldDic = new Dictionary<PatternType, string>();

        #region 열지 마시오
        fieldDic[PatternType.None] = "필드 없음";
        fieldDic[PatternType.Spade] = "물 필드";
        fieldDic[PatternType.Diamonds] = "번개 필드";
        fieldDic[PatternType.Heart] = "불 필드";
        fieldDic[PatternType.Clover] = "자연 필드";
        #endregion

        InitFieldOptions();
        fieldSetBtn.onClick.AddListener(SetField);
    }

    private void InitFieldOptions()
    {
        fieldDropdown.options.Clear();

        foreach (PatternType pattern in fieldDic.Keys)
        {
            Dropdown.OptionData op = new Dropdown.OptionData();
            op.text = fieldDic[pattern];
            op.image = GameManager.Instance.inventoryHandler.effectSprDic[pattern];

            fieldDropdown.options.Add(op);
        }

        fieldDropdown.SetValueWithoutNotify(-1);
        fieldDropdown.SetValueWithoutNotify(0);
    }

    private void SetField()
    {
        foreach (PatternType pattern in fieldDic.Keys)
        {
            if (fieldDic[pattern].Equals(fieldDropdown.captionText.text))
            {
                bf.SetFieldType(pattern);
            }
        }
    }

    public void OnReset()
    {

    }
}
