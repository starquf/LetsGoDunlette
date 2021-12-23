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

        //AddNormalAttackPiece(addNomalCnt);
    }

    // 해당 인덱스의 룰렛을 바꾸는 함수
    public void ChangePiece(int changeIdx, RulletPiece changePiece)
    {
        //pieces[changeIdx].state = PieceState.USED;
        InventoryHandler inventory = GameManager.Instance.inventorySystem;

        inventory.SetUseSkill((SkillPiece)pieces[changeIdx]);

        changePiece.transform.SetParent(transform);
        changePiece.transform.DOLocalMove(Vector3.zero, 0.35f);
        changePiece.transform.DOScale(Vector3.one, 0.35f);

        pieces[changeIdx] = changePiece;

        SetRulletSmooth();
    }

    // cnt만큼 기본 룰렛조각 추가해주는 함수
    public void AddNormalAttackPiece(int addCnt)
    {
        for (int i = 0; i < addCnt; i++)
        {
            GameObject nomalRullet = Instantiate(nomalRulletPrefab, transform);
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
