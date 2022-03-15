using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControl : MonoBehaviour
{
    public Dictionary<CCType, int> ccDic;
    public Dictionary<BuffType, int> buffDic;

    [Header("UI관련")]
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

        // 버프 설정
        foreach (BuffType buff in Enum.GetValues(typeof(BuffType)))
        {
            buffDic.Add(buff, 0);

            CCIndicator ccIndi = Instantiate(uiObj, ccUITrm).GetComponent<CCIndicator>();
            ccIndi.gameObject.SetActive(false);

            buffUIDic.Add(buff, ccIndi);
        }

        // 군중제어 설정
        foreach (CCType cc in Enum.GetValues(typeof(CCType)))
        {
            ccDic.Add(cc, 0);

            CCIndicator ccIndi = Instantiate(uiObj, ccUITrm).GetComponent<CCIndicator>();
            ccIndi.gameObject.SetActive(false);

            ccUIDic.Add(cc, ccIndi);
        }
    }

    // ================================================================ 군중제어
    public void SetCC(CCType cc, int turn)
    {
        ccDic[cc] = turn;

        ccUIDic[cc].SetText(turn);
        ccUIDic[cc].gameObject.SetActive(true);

        string messege = "";

        switch (cc)
        {
            case CCType.Stun:
                messege = "기절됨!";
                break;

            case CCType.Silence:
                messege = "침묵됨!";
                break;

            case CCType.Wound:
                messege = "상처입음!";
                break;

            case CCType.Invincibility:
                messege = "무적상태!";
                break;
        }

        // 만약 0보다 작아졌다면
        if (turn <= 0)
        {
            ccDic[cc] = 0;
            ccUIDic[cc].gameObject.SetActive(false);
        }
        else
        {
            Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
            textEffect.SetType(TextUpAnimType.Damage);
            textEffect.transform.position = ccUIDic[cc].transform.position;
            textEffect.Play(messege);
        }
    }

    public void IncreaseCCTurn(CCType cc, int turn)
    {
        ccDic[cc] += turn;
        ccUIDic[cc].gameObject.SetActive(true);

        ccUIDic[cc].SetText(ccDic[cc]);
    }

    public void DecreaseCCTurn(CCType cc)
    {
        ccDic[cc]--;

        // 만약 0보다 작아졌다면
        if (ccDic[cc] <= 0)
        {
            ccDic[cc] = 0;
            ccUIDic[cc].gameObject.SetActive(false);
        }

        ccUIDic[cc].SetText(ccDic[cc]);
    }

    // ================================================================ 버프
    public void SetBuff(BuffType buff, int value)
    {
        buffDic[buff] = value;

        buffUIDic[buff].SetText(value);
        buffUIDic[buff].gameObject.SetActive(true);
    }

    public void IncreaseBuff(BuffType buff, int turn)
    {
        buffDic[buff] += turn;
        buffUIDic[buff].gameObject.SetActive(true);

        buffUIDic[buff].SetText(buffDic[buff]);
    }

    public void DecreaseBuff(BuffType buff, int value)
    {
        buffDic[buff] -= value;

        // 만약 0보다 작아졌다면
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

    public void ResetAllCC()
    {
        foreach (CCType cc in Enum.GetValues(typeof(CCType)))
        {
            SetCC(cc, 0);
        }
    }

    public bool IsCC()
    {
        foreach (CCType cc in Enum.GetValues(typeof(CCType)))
        {
            if(ccDic[cc] > 0)
                return true;
        }

        return false;
    }
}
