using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComboManager : MonoBehaviour
{ 
    public static ComboManager Instance { get; private set; } //싱글톤은 나중에 보완

    public int maxQueueCount = 8; //8개가 넘어가면 삭제
    public Queue<ComboSlot> comboQueue = new Queue<ComboSlot>();

    [Header("몇개부터 콤보로 칠건지")]
    public int comboNum = 3; // 몇개부터 콤보로 치는지

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

    private bool CheckCombo() //콤보가 됬는지 체크함
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
