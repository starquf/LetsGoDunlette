using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComboManager : MonoBehaviour
{ 
    public static ComboManager Instance { get; private set; } //�̱����� ���߿� ����

    public int maxQueueCount = 8; //8���� �Ѿ�� ����
    public Queue<ComboSlot> comboQueue = new Queue<ComboSlot>();

    [Header("����� �޺��� ĥ����")]
    public int comboNum = 3; // ����� �޺��� ġ����

    public Transform comboSlotParent;
    public GameObject comboSlotGO;

    private void Awake()
    {
        Instance = this;
    }

    public void AddComboQueue(RulletPiece rulletPiece)
    {
        if(comboQueue.Count < maxQueueCount)
        {
            ComboSlot comboSlot = Instantiate(comboSlotGO, comboSlotParent).GetComponent<ComboSlot>();
            comboSlot.SetData(rulletPiece);

            comboQueue.Enqueue(comboSlot);
        }
        else
        {
            comboQueue.Dequeue().Delete();

            ComboSlot comboSlot = Instantiate(comboSlotGO, comboSlotParent).GetComponent<ComboSlot>();
            comboSlot.SetData(rulletPiece);
            comboQueue.Enqueue(comboSlot);
        }

        CheckCombo();
    }

    private bool CheckCombo() //�޺��� ����� üũ��
    {
        bool hasCombo = false;
        int combo = 0;
        EComboType previousType = EComboType.None;

        List<ComboSlot> comboTempList = new List<ComboSlot>();
        comboTempList = comboQueue.ToList();

        //List<ComboSlot> comboList = new List<ComboSlot>();

        for (int i = 0; i < comboTempList.Count; i++)
        {
            if (previousType.Equals(comboTempList[i].comboType))
            {
                combo++;
                comboTempList[i-1].Delete();
            }
            else
            {
                if (combo >= comboNum)
                {
                    comboTempList[i - 1].Delete();
                    hasCombo = true;
                    break;
                }
            }

            previousType = comboTempList[i].comboType;
        }

        return hasCombo;
    }
}
