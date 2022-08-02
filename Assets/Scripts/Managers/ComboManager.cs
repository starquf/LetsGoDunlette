using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance { get; private set; } //싱글톤은 나중에 보완

    // 8개
    public int maxQueueCount = 8; //8개가 넘어가면 삭제
    // 콤보 슬롯을 담는 큐
    public Queue<ComboSlot> comboQueue = new Queue<ComboSlot>();

    [Header("몇개부터 콤보로 칠건지")]
    public int comboNum = 3; // 몇개부터 콤보로 치는지

    // 콤보 슬롯을 만드는데 어디에 만들건지 트렌스폼
    public Transform comboSlotParent;
    // 콤보 슬롯 프리펩
    public GameObject comboSlotGO;

    private void Awake()
    {
        Instance = this;
    }

    // 콤보를 쌓게 하는 함수
    public void AddComboQueue(RulletPiece rulletPiece)
    {
        // maxQueueCount를 넘었다면
        if (comboQueue.Count >= maxQueueCount)
        {
            // 처음에 들어온 순으로 없엔다
            comboQueue.Dequeue().Delete();
        }

        // 콤보 슬롯 인스턴스
        ComboSlot comboSlot = Instantiate(comboSlotGO, comboSlotParent).GetComponent<ComboSlot>();
        // 콤보 슬롯의 정보를 넣어준다
        comboSlot.SetData(rulletPiece);
        // 큐에 추가
        comboQueue.Enqueue(comboSlot);

        //CheckCombo();
    }

    private bool CheckCombo() //콤보가 됬는지 체크함
    {
        // 콤보가 있냐?
        bool hasCombo = false;
        // 콤보 카운트
        int combo = 0;

        // 이전 거의 타입
        ElementalType previousType = ElementalType.None;

        // 큐를 리스트로 변환하는 거
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
