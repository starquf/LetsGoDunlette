using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class SkillRullet : Rullet
{
    // 기본 룰렛 조각 프리팹
    public GameObject nomalRulletPrefab;

    protected override void Start()
    {
        GetComponentsInChildren(pieces);

        SetRullet();

        int sizeSum = 0;
        for (int i = 0; i < pieces.Count; i++)
        {
            sizeSum += pieces[i].Size;
        }

        int addNomalCnt = (maxSize - sizeSum) / 6;

        AddNormalAttackPiece(addNomalCnt);
    }

    // 해당 인덱스의 룰렛을 바꾸는 함수
    public void ChangePiece(int changeIdx, RulletPiece changePiece)
    {
        Destroy(pieces[changeIdx].gameObject);
        pieces[changeIdx] = changePiece;
        SetRulletSmooth();
    }

    // cnt만큼 기본 룰렛조각 추가해주는 함수
    public void AddNormalAttackPiece(int addCnt)
    {
        for (int i = 0; i < addCnt; i++)
        {
            GameObject nomalRullet = Instantiate(nomalRulletPrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
            pieces.Add(nomalRullet.GetComponent<RulletPiece>());
            nomalRullet.transform.localPosition = Vector3.zero;
        }

        SetRullet();
    }

    protected override void CastDefault()
    {
        result = null;
    }
}
