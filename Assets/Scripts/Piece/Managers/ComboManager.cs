using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance { get; private set; } //�̱����� ���߿� ����

    // 8��
    public int maxQueueCount = 8; //8���� �Ѿ�� ����
    // �޺� ������ ��� ť
    public Queue<ComboSlot> comboQueue = new Queue<ComboSlot>();

    [Header("����� �޺��� ĥ����")]
    public int comboNum = 3; // ����� �޺��� ġ����

    // �޺� ������ ����µ� ��� ������� Ʈ������
    public Transform comboSlotParent;
    // �޺� ���� ������
    public GameObject comboSlotGO;

    private void Awake()
    {
        Instance = this;
    }

    // �޺��� �װ� �ϴ� �Լ�
    public void AddComboQueue(RulletPiece rulletPiece)
    {
        // maxQueueCount�� �Ѿ��ٸ�
        if (comboQueue.Count >= maxQueueCount)
        {
            // ó���� ���� ������ ������
            comboQueue.Dequeue().Delete();
        }

        // �޺� ���� �ν��Ͻ�
        ComboSlot comboSlot = Instantiate(comboSlotGO, comboSlotParent).GetComponent<ComboSlot>();
        // �޺� ������ ������ �־��ش�
        comboSlot.SetData(rulletPiece);
        // ť�� �߰�
        comboQueue.Enqueue(comboSlot);

        //CheckCombo();
    }

    private bool CheckCombo() //�޺��� ����� üũ��
    {
        // �޺��� �ֳ�?
        bool hasCombo = false;
        // �޺� ī��Ʈ
        int combo = 0;

        // ���� ���� Ÿ��
        ElementalType previousType = ElementalType.None;

        // ť�� ����Ʈ�� ��ȯ�ϴ� ��
        List<ComboSlot> comboTempList = new List<ComboSlot>();
        comboTempList = comboQueue.ToList();

        for (int i = 0; i < comboTempList.Count; i++)
        {
            if (previousType.Equals(comboTempList[i].comboType))
            {
                combo++;
                comboTempList[i - 1].Delete();
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
