using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControl : MonoBehaviour
{
    public Dictionary<CCType, int> ccDic;
    public Dictionary<BuffType, int> buffDic;

    [Header("UI����")]
    public Transform ccUITrm;
    public GameObject uiObj;

    public Dictionary<CCType, CCIndicator> ccUIDic;
    public List<Sprite> ccIcons = new List<Sprite>();

    public Dictionary<BuffType, CCIndicator> buffUIDic;
    public List<Sprite> buffIcons = new List<Sprite>();

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        for (int i = 0; i < ccIcons.Count; i++)
        {
            ccUIDic[(CCType)i].SetImg(ccIcons[i]);
            ccUIDic[(CCType)i].SetText(0);
        }

        for (int i = 0; i < buffIcons.Count; i++)
        {
            buffUIDic[(BuffType)i].SetImg(buffIcons[i]);
            buffUIDic[(BuffType)i].SetText(0);
        }
    }

    private void Init()
    {
        ccDic = new Dictionary<CCType, int>();
        ccUIDic = new Dictionary<CCType, CCIndicator>();

        buffDic = new Dictionary<BuffType, int>();
        buffUIDic = new Dictionary<BuffType, CCIndicator>();

        // ���� ����
        foreach (BuffType buff in Enum.GetValues(typeof(BuffType)))
        {
            buffDic.Add(buff, 0);

            CCIndicator ccIndi = Instantiate(uiObj, ccUITrm).GetComponent<CCIndicator>();
            ccIndi.gameObject.SetActive(false);

            buffUIDic.Add(buff, ccIndi);
        }

        // �������� ����
        foreach (CCType cc in Enum.GetValues(typeof(CCType)))
        {
            ccDic.Add(cc, 0);

            CCIndicator ccIndi = Instantiate(uiObj, ccUITrm).GetComponent<CCIndicator>();
            ccIndi.gameObject.SetActive(false);

            ccUIDic.Add(cc, ccIndi);
        }
    }

    // ================================================================ ��������
    public void SetCC(CCType cc, int turn)
    {
        ccDic[cc] = turn;

        ccUIDic[cc].SetText(turn);
        ccUIDic[cc].gameObject.SetActive(true);
    }

    public void IncreaseCCTurn(CCType cc, int turn)
    {
        ccDic[cc] += turn;
        ccUIDic[cc].SetText(ccDic[cc]);
    }

    public void DecreaseCCTurn(CCType cc)
    {
        ccDic[cc]--;

        // ���� 0���� �۾����ٸ�
        if (ccDic[cc] <= 0)
        {
            ccDic[cc] = 0;
            ccUIDic[cc].gameObject.SetActive(false);
        }

        ccUIDic[cc].SetText(ccDic[cc]);
    }

    // ================================================================ ����
    public void SetBuff(BuffType buff, int value)
    {
        buffDic[buff] = value;

        buffUIDic[buff].SetText(value);
        buffUIDic[buff].gameObject.SetActive(true);
    }

    public void IncreaseBuff(BuffType buff, int turn)
    {
        buffDic[buff] += turn;
        buffUIDic[buff].SetText(buffDic[buff]);
    }

    public void DecreaseBuff(BuffType buff, int value)
    {
        buffDic[buff] -= value;

        // ���� 0���� �۾����ٸ�
        if (buffDic[buff] <= 0)
        {
            buffDic[buff] = 0;
            buffUIDic[buff].gameObject.SetActive(false);
        }

        buffUIDic[buff].SetText(buffDic[buff]);
    }

    public void RemoveBuff(BuffType buff)
    {
        buffDic[buff] = 0;
        buffUIDic[buff].gameObject.SetActive(false);

        buffUIDic[buff].SetText(buffDic[buff]);
    }

    public void DecreaseAllTurn()
    {
        foreach (CCType cc in Enum.GetValues(typeof(CCType)))
        {
            DecreaseCCTurn(cc);
        }
    }
}
