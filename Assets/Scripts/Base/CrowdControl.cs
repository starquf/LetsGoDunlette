using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControl : MonoBehaviour
{
    public Dictionary<CCType, int> ccDic;

    [Header("UI관련")]
    public Transform ccUITrm;
    public GameObject uiObj;

    public Dictionary<CCType, CCIndicator> ccUIDic;
    public List<Sprite> ccIcons = new List<Sprite>();

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        for (int i = 0; i < ccIcons.Count; i++)
        {
            ccUIDic[(CCType)i].SetCCImg(ccIcons[i]);
            ccUIDic[(CCType)i].SetTurn(0);
        }
    }

    private void Init()
    {
        ccDic = new Dictionary<CCType, int>();
        ccUIDic = new Dictionary<CCType, CCIndicator>();

        ccDic.Add(CCType.Stun, 0);
        ccDic.Add(CCType.Silence, 0);
        ccDic.Add(CCType.Exhausted, 0);
        ccDic.Add(CCType.Wound, 0);

        foreach (CCType cc in Enum.GetValues(typeof(CCType)))
        {
            CCIndicator ccIndi = Instantiate(uiObj, ccUITrm).GetComponent<CCIndicator>();
            ccIndi.gameObject.SetActive(false);

            ccUIDic.Add(cc, ccIndi);
        }
    }

    public void SetCC(CCType cc, int turn)
    {
        // 이미 cc가 존재한다면
        if (ccDic[cc] > 0) return;

        ccDic[cc] = turn;

        ccUIDic[cc].SetTurn(turn);
        ccUIDic[cc].gameObject.SetActive(true);
    }

    public void IncreaseTurn(CCType cc, int turn)
    {
        ccDic[cc] += turn;
        ccUIDic[cc].SetTurn(ccDic[cc]);
    }

    public void DecreaseTurn(CCType cc)
    {
        ccDic[cc]--;

        // 만약 0보다 작아졌다면
        if (ccDic[cc] <= 0)
        {
            ccDic[cc] = 0;
            ccUIDic[cc].gameObject.SetActive(false);
        }

        ccUIDic[cc].SetTurn(ccDic[cc]);
    }

    public void DecreaseAllTurn()
    {
        foreach (CCType cc in Enum.GetValues(typeof(CCType)))
        {
            DecreaseTurn(cc);
        }
    }
}
