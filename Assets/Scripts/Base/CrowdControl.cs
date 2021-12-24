using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControl : MonoBehaviour
{
    public Dictionary<CCType, int> ccDic;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        ccDic = new Dictionary<CCType, int>();

        ccDic.Add(CCType.Stun, 0);
        ccDic.Add(CCType.Silence, 0);
        ccDic.Add(CCType.Exhausted, 0);
        ccDic.Add(CCType.Wound, 0);
    }

    public CCType GetCurrentCC()
    {
        // ������ ���� cc, �⺻�� None
        CCType currentCC = CCType.None;

        foreach (var cc in ccDic.Keys)
        {
            // �� ī��Ʈ �ϳ� ���̱�
            DecreaseTurn(cc);

            // cc�� �����Ѵٸ�
            if (ccDic[cc] > 0)
            {
                // ���� cc�� �߰�
                currentCC |= cc;
            }
        }

        return currentCC;
    }

    public void SetCC(CCType cc, int turn)
    {
        ccDic[cc] = turn;
    }

    public void IncreaseTurn(CCType cc, int turn)
    {
        ccDic[cc] += turn;
    }

    public void DecreaseTurn(CCType cc)
    {
        ccDic[cc]--;

        // ���� 0���� �۾����ٸ�
        if (ccDic[cc] < 0)
        {
            ccDic[cc] = 0;
        }
    }
}
