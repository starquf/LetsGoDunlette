using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugFieldPanelHandler : MonoBehaviour, IDebugPanel
{
    private BattleFieldHandler bf;

    [Header("�ʵ� ���õ�")]
    public Dropdown fieldDropdown;
    public Button fieldSetBtn;

    private Dictionary<PatternType, string> fieldDic;

    public void Init()
    {
        bf = GameManager.Instance.battleFieldHandler;

        fieldDic = new Dictionary<PatternType, string>();

        #region ���� ���ÿ�
        fieldDic[PatternType.None] = "�ʵ� ����";
        fieldDic[PatternType.Spade] = "�� �ʵ�";
        fieldDic[PatternType.Diamonds] = "���� �ʵ�";
        fieldDic[PatternType.Heart] = "�� �ʵ�";
        fieldDic[PatternType.Clover] = "�ڿ� �ʵ�";
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
