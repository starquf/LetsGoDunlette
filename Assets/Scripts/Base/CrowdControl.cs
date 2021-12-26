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

    public void SetCC(CCType cc, int turn)
    {
        // 이미 cc가 존재한다면
        if (ccDic[cc] > 0) return;

        ccDic[cc] = turn;
    }

    public void IncreaseTurn(CCType cc, int turn)
    {
        ccDic[cc] += turn;
    }

    public void DecreaseTurn(CCType cc)
    {
        ccDic[cc]--;

        // 만약 0보다 작아졌다면
        if (ccDic[cc] < 0)
        {
            ccDic[cc] = 0;
        }
    }

    public void DecreaseAllTurn()
    {
        foreach (CCType cc in Enum.GetValues(typeof(CCType)))
        {
            DecreaseTurn(cc);
        }
    }
}
